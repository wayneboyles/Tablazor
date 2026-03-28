using Tablazor.Enums;

namespace Tablazor.Models;

/// <summary>
/// Represents a single option in a <see cref="TabFieldType.Select"/> or
/// <see cref="TabFieldType.Radio"/> field.
/// </summary>
public sealed class SelectOption
{
    /// <summary>The value submitted to the model when this option is selected.</summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>The human-readable text displayed to the user.</summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>When <c>true</c>, the option is visible but cannot be selected.</summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// Optional group label. Options sharing the same <see cref="Group"/> string
    /// are wrapped in an HTML <c>&lt;optgroup&gt;</c> element inside a select list.
    /// Ignored for radio groups.
    /// </summary>
    public string? Group { get; set; }

    /// <summary>Initializes an empty <see cref="SelectOption"/>.</summary>
    public SelectOption() { }

    /// <summary>Initializes a <see cref="SelectOption"/> with the specified value and display text.</summary>
    public SelectOption(string value, string text, bool disabled = false, string? group = null)
    {
        Value = value;
        Text = text;
        Disabled = disabled;
        Group = group;
    }

    /// <summary>Convenience factory: creates options where value and text are the same string.</summary>
    public static SelectOption From(string valueAndText) => new(valueAndText, valueAndText);

    /// <summary>
    /// Convenience factory: creates a list of <see cref="SelectOption"/> from a sequence of
    /// (value, text) tuples.
    /// </summary>
    public static List<SelectOption> FromPairs(IEnumerable<(string value, string text)> pairs)
        => pairs.Select(p => new SelectOption(p.value, p.text)).ToList();
}
