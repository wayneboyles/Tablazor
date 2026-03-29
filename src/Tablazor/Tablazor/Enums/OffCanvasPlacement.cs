using Tablazor.Attributes;

namespace Tablazor.Enums;

/// <summary>
/// Controls which edge of the viewport the <c>TabOffCanvas</c> panel slides in from.
/// </summary>
public enum OffCanvasPlacement
{
    /// <summary>
    /// Slides in from the left edge of the viewport.
    /// </summary>
    [CssClassName("start")]
    Start,

    /// <summary>
    /// Slides in from the right edge of the viewport.
    /// </summary>
    [CssClassName("end")]
    End,

    /// <summary>
    /// Slides in from the top edge of the viewport.
    /// </summary>
    [CssClassName("top")]
    Top,

    /// <summary>
    /// Slides in from the bottom edge of the viewport.
    /// </summary>
    [CssClassName("bottom")]
    Bottom
}
