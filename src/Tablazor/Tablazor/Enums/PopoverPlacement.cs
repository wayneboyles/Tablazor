using Tablazor.Attributes;

namespace Tablazor.Enums;

/// <summary>
/// Specifies the placement of a popover relative to its trigger element.
/// </summary>
public enum PopoverPlacement
{
    /// <summary>
    /// The popover appears above the trigger element. This is the default.
    /// </summary>
    [CssClassName("top")]
    Top,

    /// <summary>
    /// The popover appears below the trigger element.
    /// </summary>
    [CssClassName("bottom")]
    Bottom,

    /// <summary>
    /// The popover appears to the left of the trigger element (start in LTR layouts).
    /// </summary>
    [CssClassName("start")]
    Start,

    /// <summary>
    /// The popover appears to the right of the trigger element (end in LTR layouts).
    /// </summary>
    [CssClassName("end")]
    End
}
