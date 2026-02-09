using Tablazor.Attributes;

namespace Tablazor.Enums;

/// <summary>
/// Represents the transition effect between carousel slides.
/// </summary>
public enum CarouselTransition
{
    /// <summary>
    /// Horizontal slide transition (default).
    /// </summary>
    Slide,

    /// <summary>
    /// Fade in/out transition.
    /// </summary>
    [CssClassName("carousel-fade")]
    Fade
}
