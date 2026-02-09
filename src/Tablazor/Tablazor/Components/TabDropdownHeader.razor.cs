using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// Represents a header label within a <see cref="TabDropdownMenu"/>.
/// Used to provide context or categorize groups of dropdown items.
/// </summary>
public partial class TabDropdownHeader : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the child content of the dropdown header.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("dropdown-header")
            .AddClass(CssClass)
            .Build();
    }
}
