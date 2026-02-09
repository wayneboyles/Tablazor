using Tablazor.Attributes;

namespace Tablazor.Enums;

/// <summary>
/// Represents the style of carousel indicators.
/// </summary>
public enum CarouselIndicatorStyle
{
    /// <summary>
    /// Standard numbered indicator buttons.
    /// </summary>
    Default,

    /// <summary>
    /// Circular dot indicators.
    /// </summary>
    [CssClassName("carousel-indicators-dot")]
    Dot,

    /// <summary>
    /// Thumbnail image indicators.
    /// </summary>
    [CssClassName("carousel-indicators-thumb")]
    Thumbnail
}
