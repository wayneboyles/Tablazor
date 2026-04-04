namespace Tablazor.Enums;

/// <summary>
/// Specifies how a popover is triggered.
/// </summary>
public enum PopoverTrigger
{
    /// <summary>
    /// The popover is shown and hidden by clicking the trigger element.
    /// Click outside to dismiss.
    /// </summary>
    Click,

    /// <summary>
    /// The popover is shown on mouse enter and hidden on mouse leave.
    /// </summary>
    Hover,

    /// <summary>
    /// The popover is shown when the trigger element receives focus and hidden when it loses focus.
    /// </summary>
    Focus,

    /// <summary>
    /// The popover is controlled programmatically via <c>ShowAsync</c>, <c>HideAsync</c>, and <c>ToggleAsync</c>.
    /// </summary>
    Manual
}
