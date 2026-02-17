namespace Tablazor.Enums;

/// <summary>
/// Specifies which buttons are displayed in a <see cref="Tablazor.Components.TabDialog"/>.
/// This is a flags enum, allowing combinations of buttons.
/// </summary>
[Flags]
public enum DialogButtons
{
    /// <summary>
    /// Displays an OK button.
    /// </summary>
    Ok = 1,

    /// <summary>
    /// Displays a Cancel button.
    /// </summary>
    Cancel = 2,

    /// <summary>
    /// Displays a Yes button.
    /// </summary>
    Yes = 4,

    /// <summary>
    /// Displays a No button.
    /// </summary>
    No = 8,

    /// <summary>
    /// Displays OK and Cancel buttons.
    /// </summary>
    OkCancel = Ok | Cancel,

    /// <summary>
    /// Displays Yes and No buttons.
    /// </summary>
    YesNo = Yes | No,

    /// <summary>
    /// Displays Yes, No, and Cancel buttons.
    /// </summary>
    YesNoCancel = Yes | No | Cancel
}
