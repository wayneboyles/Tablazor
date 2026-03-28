namespace Tablazor.Models;

/// <summary>
/// Represents a logical visual group of form rows under an optional section heading.
/// Sections are created when a model property carries a
/// <see cref="Attributes.TabSectionAttribute"/>; that property's section starts a new group
/// that continues until the next <c>TabSectionAttribute</c> is encountered.
/// </summary>
internal sealed class FormSection
{
    /// <summary>
    /// The section heading text, or <c>null</c> for the implicit default section that
    /// precedes any explicitly-labelled section.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>Optional subtitle rendered below the title in muted text.</summary>
    public string? Description { get; init; }

    /// <summary>When <c>true</c>, the section body can be collapsed by clicking the header.</summary>
    public bool Collapsible { get; init; }

    /// <summary>Initial collapsed state when <see cref="Collapsible"/> is <c>true</c>.</summary>
    public bool InitiallyCollapsed { get; init; }

    /// <summary>Additional CSS class(es) for the section wrapper element.</summary>
    public string? CssClass { get; init; }

    /// <summary>The ordered rows of fields belonging to this section.</summary>
    public List<FormRow> Rows { get; init; } = new();

    /// <summary><c>true</c> when a non-blank title is set.</summary>
    public bool HasTitle => !string.IsNullOrWhiteSpace(Title);
}