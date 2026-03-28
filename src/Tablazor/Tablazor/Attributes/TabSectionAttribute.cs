namespace Tablazor.Attributes;

/// <summary>
/// Marks the start of a named visual section within the form.
/// The section encompasses all subsequent properties (in their rendered order) until
/// the next property decorated with <see cref="TabSectionAttribute"/> is encountered.
/// </summary>
/// <remarks>
/// Sections are rendered as a heading (and optional description) above their fields.
/// When <see cref="Collapsible"/> is <c>true</c>, clicking the heading toggles visibility.
/// <para>Example:</para>
/// <code>
/// [TabSection("Personal Details")]
/// public string FirstName { get; set; }
/// public string LastName { get; set; }
///
/// [TabSection("Contact", Description = "How we can reach you")]
/// public string Email { get; set; }
/// public string Phone { get; set; }
/// </code>
/// </remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class TabSectionAttribute : Attribute
{
    /// <summary>The section heading text displayed above the section's fields.</summary>
    public string Title { get; }

    /// <summary>
    /// Optional subtitle or description rendered below the section title
    /// using Tabler's <c>.text-muted</c> style.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// When <c>true</c>, the section body can be collapsed/expanded by clicking the header.
    /// A chevron icon is shown to indicate the current state.
    /// </summary>
    public bool Collapsible { get; set; }

    /// <summary>
    /// When <see cref="Collapsible"/> is <c>true</c>, controls whether the section
    /// starts in the collapsed state. Defaults to <c>false</c> (expanded).
    /// </summary>
    public bool InitiallyCollapsed { get; set; }

    /// <summary>
    /// Additional CSS class(es) applied to the section's wrapper element.
    /// </summary>
    public string? CssClass { get; set; }

    /// <summary>
    /// Creates a new <see cref="TabSectionAttribute"/> with the given heading.
    /// </summary>
    /// <param name="title">The section heading text.</param>
    public TabSectionAttribute(string title) => Title = title;
}