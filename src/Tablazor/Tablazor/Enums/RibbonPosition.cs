using Tablazor.Attributes;
using Tablazor.Components;

namespace Tablazor.Enums;

/// <summary>
/// Determines the position of a <see cref="TabRibbon"/> on its parent element.
/// </summary>
public enum RibbonPosition
{
    /// <summary>
    /// Top-right corner (default).
    /// </summary>
    [CssClassName("ribbon-top ribbon-end")]
    TopEnd,

    /// <summary>
    /// Top-left corner.
    /// </summary>
    [CssClassName("ribbon-top ribbon-start")]
    TopStart,

    /// <summary>
    /// Bottom-right corner.
    /// </summary>
    [CssClassName("ribbon-bottom ribbon-end")]
    BottomEnd,

    /// <summary>
    /// Bottom-left corner.
    /// </summary>
    [CssClassName("ribbon-bottom ribbon-start")]
    BottomStart,

    /// <summary>
    /// Top edge only (no horizontal constraint).
    /// </summary>
    [CssClassName("ribbon-top")]
    Top,

    /// <summary>
    /// Bottom edge only (no horizontal constraint).
    /// </summary>
    [CssClassName("ribbon-bottom")]
    Bottom,

    /// <summary>
    /// Left side only (no vertical constraint).
    /// </summary>
    [CssClassName("ribbon-start")]
    Start,

    /// <summary>
    /// Right side only (no vertical constraint).
    /// </summary>
    [CssClassName("ribbon-end")]
    End,
}
