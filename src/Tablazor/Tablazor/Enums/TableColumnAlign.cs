using Tablazor.Attributes;

namespace Tablazor.Enums;

/// <summary>
/// Specifies the horizontal alignment of a table column's content.
/// </summary>
public enum TableColumnAlign
{
    /// <summary>
    /// Left-aligned (default).
    /// </summary>
    [CssClassName("text-start")]
    Start,

    /// <summary>
    /// Center-aligned.
    /// </summary>
    [CssClassName("text-center")]
    Center,

    /// <summary>
    /// Right-aligned.
    /// </summary>
    [CssClassName("text-end")]
    End,
}
