using Tablazor.Attributes;

namespace Tablazor.Enums;

/// <summary>
/// Represents the position where toasts will be displayed on the screen.
/// </summary>
public enum ToastPosition
{
    /// <summary>
    /// Top right corner of the viewport.
    /// </summary>
    [CssClassName("top-0 end-0")]
    TopRight,

    /// <summary>
    /// Top left corner of the viewport.
    /// </summary>
    [CssClassName("top-0 start-0")]
    TopLeft,

    /// <summary>
    /// Top center of the viewport.
    /// </summary>
    [CssClassName("top-0 start-50 translate-middle-x")]
    TopCenter,

    /// <summary>
    /// Bottom right corner of the viewport.
    /// </summary>
    [CssClassName("bottom-0 end-0")]
    BottomRight,

    /// <summary>
    /// Bottom left corner of the viewport.
    /// </summary>
    [CssClassName("bottom-0 start-0")]
    BottomLeft,

    /// <summary>
    /// Bottom center of the viewport.
    /// </summary>
    [CssClassName("bottom-0 start-50 translate-middle-x")]
    BottomCenter
}
