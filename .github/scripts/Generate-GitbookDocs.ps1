<#
.SYNOPSIS
    Generates GitBook-compatible Markdown documentation from C#/Blazor XML comments
    
.DESCRIPTION
    Extracts XML documentation comments from a .NET project, generates organized
    Markdown files, and prepares them for GitBook export. Creates SUMMARY.md
    (table of contents) and README.md for proper GitBook integration.
    
.PARAMETER ProjectPath
    Path to the .csproj file containing the RCL
    
.PARAMETER OutputPath
    Output directory for GitBook documentation (default: ./docs)
    
.EXAMPLE
    .\Generate-GitbookDocs.ps1 -ProjectPath "./src/MyRcl.csproj" -OutputPath "./docs"
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$ProjectPath,
    
    [Parameter(Mandatory = $false)]
    [string]$OutputPath = "./docs",
    
)

# Enable strict error handling
$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"

# ============================================================================
# Helper Functions
# ============================================================================

function Write-Status {
    param([string]$Message)
    Write-Host "📚 $Message" -ForegroundColor Cyan
}

function Write-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor Green
}

function Write-Error-Custom {
    param([string]$Message)
    Write-Host "✗ $Message" -ForegroundColor Red
}

function Get-XmlDocPath {
    param([string]$ProjectPath)
    
    $proj = [xml](Get-Content $ProjectPath)

    # Try to find DocumentationFile — SDK-style .csproj files have no XML namespace
    $docFile = $proj.SelectSingleNode("//DocumentationFile")?.InnerText
    
    if ($docFile) {
        $basePath = Split-Path $ProjectPath
        return Join-Path $basePath $docFile
    }
    
    # Fallback: assume standard bin/Debug/net8.0/ProjectName.xml
    $projName = [System.IO.Path]::GetFileNameWithoutExtension($ProjectPath)
    $fallback = Join-Path (Split-Path $ProjectPath) "bin/Debug/net8.0/$projName.xml"
    
    if (Test-Path $fallback) {
        return $fallback
    }
    
    return $null
}

function Parse-XmlDocumentation {
    param([string]$XmlPath)
    
    if (-not (Test-Path $XmlPath)) {
        throw "XML documentation file not found: $XmlPath"
    }
    
    Write-Status "Parsing XML documentation from $XmlPath"
    
    [xml]$xmlDoc = Get-Content $XmlPath
    $members = @{}
    
    foreach ($member in $xmlDoc.doc.members.member) {
        $name = $member.name
        $summary = $member.summary?.InnerText.Trim() -replace '\s+', ' '
        $remarks = $member.remarks?.InnerText.Trim()
        $example = $member.example?.InnerText.Trim()
        $returns = $member.returns?.InnerText.Trim() -replace '\s+', ' '
        
        # Parse parameters
        $params = @()
        foreach ($param in $member.param) {
            $params += @{
                name        = $param.name
                description = $param.InnerText.Trim() -replace '\s+', ' '
            }
        }
        
        $members[$name] = @{
            summary    = $summary
            remarks    = $remarks
            example    = $example
            returns    = $returns
            parameters = $params
            type       = $name.Substring(0, 1)  # T=Type, M=Method, P=Property, F=Field
            fullName   = $name.Substring(2)    # Remove prefix
        }
    }
    
    Write-Success "Parsed $($members.Count) documentation members"
    return $members
}

function Group-ByNamespace {
    param([hashtable]$Members)
    
    $grouped = @{}
    
    foreach ($key in $Members.Keys) {
        $member   = $Members[$key]
        $fullName = $member.fullName
        $segments = $fullName -split '\.'

        # Types (T:): namespace is the second-to-last segment.
        # Members (M:/P:/F:/E:): the last segment is the member name, second-to-last is the
        # class name, so the namespace sits at index -3.
        $nsIndex  = if ($member.type -eq 'T') { -2 } else { -3 }
        $namespace = if ($segments.Count -ge [Math]::Abs($nsIndex)) {
            $segments[$nsIndex]
        } else {
            $segments[0]
        }

        if (-not $grouped[$namespace]) {
            $grouped[$namespace] = @()
        }

        $grouped[$namespace] += @{
            key  = $key
            data = $member
            name = $segments[-1]
        }
    }
    
    return $grouped
}

function Escape-MarkdownSpecialChars {
    param([string]$Text)
    
    if ([string]::IsNullOrEmpty($Text)) { return "" }
    
    # Escape markdown special characters; backticks are left unescaped to preserve inline code spans
    $Text = $Text -replace '([\\*_{}[\]()#+\-.!])', '\$1'

    return $Text
}

function Generate-TypeMarkdown {
    param(
        [hashtable]$Member,
        [string]$Name
    )
    
    $md = @()
    $md += "# $Name"
    $md += ""
    
    if ($Member.summary) {
        $md += $Member.summary
        $md += ""
    }
    
    if ($Member.remarks) {
        $md += "## Remarks"
        $md += ""
        $md += $Member.remarks
        $md += ""
    }
    
    if ($Member.parameters.Count -gt 0) {
        $md += "## Parameters"
        $md += ""
        $md += "| Name | Description |"
        $md += "|------|-------------|"
        
        foreach ($param in $Member.parameters) {
            $desc = Escape-MarkdownSpecialChars $param.description
            $md += "| ``$($param.name)`` | $desc |"
        }
        $md += ""
    }
    
    if ($Member.returns) {
        $md += "## Returns"
        $md += ""
        $md += $Member.returns
        $md += ""
    }
    
    if ($Member.example) {
        $md += "## Example"
        $md += ""
        $md += "``````csharp"
        $md += $Member.example
        $md += "``````"
        $md += ""
    }
    
    return $md -join "`n"
}

function Create-DirectoryStructure {
    param([string]$OutputPath)
    
    $dirs = @(
        $OutputPath,
        (Join-Path $OutputPath "components"),
        (Join-Path $OutputPath "services"),
        (Join-Path $OutputPath "models"),
        (Join-Path $OutputPath "utilities")
    )
    
    foreach ($dir in $dirs) {
        if (-not (Test-Path $dir)) {
            New-Item -ItemType Directory -Path $dir -Force | Out-Null
        }
    }
}

function Generate-ReadmeFile {
    param([string]$OutputPath, [string]$ProjectName)
    
    $readmeContent = @"
# $ProjectName Documentation

Welcome to the complete API reference for **$ProjectName**.

This documentation is automatically generated from source code XML comments and kept in sync with every release.

## Getting Started

- Browse the [API Reference](components/) for detailed component documentation
- Check [Services](services/) for service layer documentation
- Review [Models](models/) for data structure definitions
- Find [Utilities](utilities/) for helper functions and extensions

## Navigation

Use the sidebar to explore the complete API. Each entry includes:
- **Summary**: Brief description
- **Remarks**: Detailed information and usage notes
- **Parameters**: Method parameters with descriptions
- **Returns**: Return type information
- **Examples**: Code samples showing typical usage

---

*Last updated: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')*
"@
    
    Set-Content -Path (Join-Path $OutputPath "README.md") -Value $readmeContent -Encoding UTF8
}

function Generate-SummaryFile {
    param([string]$OutputPath, [hashtable]$Grouped)
    
    $summary = @()
    $summary += "# Table of Contents"
    $summary += ""
    $summary += "- [Home](./README.md)"
    $summary += ""
    
    $categories = @{
        "Components"  = "components"
        "Services"    = "services"
        "Models"      = "models"
        "Utilities"   = "utilities"
    }
    
    foreach ($category in $categories.GetEnumerator()) {
        $categoryName = $category.Key
        $categoryPath = $category.Value
        
        $categoryItems = $Grouped.Keys | Where-Object { $_ -like "*$categoryName*" }
        
        if (@($categoryItems).Count -gt 0) {
            $summary += "- **$categoryName**"

            foreach ($ns in ($categoryItems | Sort-Object)) {
                $summary += "  - [$ns]($categoryPath/$($ns.ToLower())/)"

                foreach ($item in $Grouped[$ns]) {
                    $fileName = $item.name -replace '\s+', '-'
                    $summary += "    - [$($item.name)]($categoryPath/$($ns.ToLower())/$fileName.md)"
                }
            }

            $summary += ""
        }
    }
    
    Set-Content -Path (Join-Path $OutputPath "SUMMARY.md") -Value ($summary -join "`n") -Encoding UTF8
}

function Export-MarkdownFiles {
    param(
        [hashtable]$Members,
        [string]$OutputPath
    )
    
    Write-Status "Generating Markdown files"
    
    $grouped = Group-ByNamespace $Members
    $fileCount = 0
    
    foreach ($namespace in $grouped.Keys | Sort-Object) {
        $nsFolder = Join-Path $OutputPath ($namespace.ToLower())
        
        if (-not (Test-Path $nsFolder)) {
            New-Item -ItemType Directory -Path $nsFolder -Force | Out-Null
        }
        
        # Create namespace index
        $nsIndex = @("# $namespace", "")
        
        foreach ($item in $grouped[$namespace]) {
            $fileName = $item.name -replace '\s+', '-'
            $filePath = Join-Path $nsFolder "$fileName.md"
            
            $markdown = Generate-TypeMarkdown -Member $item.data -Name $item.name
            Set-Content -Path $filePath -Value $markdown -Encoding UTF8
            
            $nsIndex += "- [$($item.name)]($fileName.md)"
            $fileCount++
        }
        
        # Write namespace index
        $indexPath = Join-Path $nsFolder "README.md"
        Set-Content -Path $indexPath -Value ($nsIndex -join "`n") -Encoding UTF8
    }
    
    Write-Success "Generated $fileCount markdown files"
}

# ============================================================================
# Main Execution
# ============================================================================

try {
    Write-Host ""
    Write-Host "╔════════════════════════════════════════════════════════════╗" -ForegroundColor Magenta
    Write-Host "║         GitBook Documentation Generator for .NET           ║" -ForegroundColor Magenta
    Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Magenta
    Write-Host ""
    
    # Validate inputs
    if (-not (Test-Path $ProjectPath)) {
        throw "Project file not found: $ProjectPath"
    }
    
    $projectName = [System.IO.Path]::GetFileNameWithoutExtension($ProjectPath)
    
    # Get or build XML doc
    $xmlPath = Get-XmlDocPath $ProjectPath
    
    if (-not $xmlPath) {
        Write-Status "Building project to generate XML documentation..."
        $binPath = Split-Path $ProjectPath
        Push-Location $binPath
        dotnet build --configuration Release
        if ($LASTEXITCODE -ne 0) { throw "dotnet build failed with exit code $LASTEXITCODE" }
        Pop-Location
        $xmlPath = Get-XmlDocPath $ProjectPath
    }
    
    if (-not $xmlPath) {
        throw "Could not locate XML documentation file. Ensure <GenerateDocumentationFile>true</GenerateDocumentationFile> is set in your .csproj"
    }
    
    # Create output structure
    Create-DirectoryStructure $OutputPath
    
    # Parse documentation
    $members = Parse-XmlDocumentation $xmlPath
    
    # Generate files
    Generate-ReadmeFile $OutputPath $projectName
    Export-MarkdownFiles $members $OutputPath
    Generate-SummaryFile $OutputPath (Group-ByNamespace $members)
    
    Write-Host ""
    Write-Success "Documentation generation complete!"
    Write-Host "📁 Output directory: $(Resolve-Path $OutputPath)" -ForegroundColor Green
    Write-Host ""
    
}
catch {
    Write-Error-Custom "Error: $_"
    Write-Host "Stack trace: $($_.ScriptStackTrace)" -ForegroundColor Gray
    exit 1
}