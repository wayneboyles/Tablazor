using Tablazor.Attributes;

namespace Tablazor.Enums;

/// <summary>
/// Determines the size modifier applied to a placeholder element.
/// </summary>
public enum PlaceholderSize
{
    /// <summary>
    /// Default placeholder size — no size modifier class is applied.
    /// </summary>
    Default,

    /// <summary>
    /// Extra-small placeholder.
    /// </summary>
    [CssClassName("xs")]
    ExtraSmall,

    /// <summary>
    /// Small placeholder.
    /// </summary>
    [CssClassName("sm")]
    Small,

    /// <summary>
    /// Large placeholder.
    /// </summary>
    [CssClassName("lg")]
    Large,
}
