namespace Tablazor.Enums;

/// <summary>
/// Represents the result of a dialog interaction, indicating which button the user clicked.
/// </summary>
public enum DialogResult
{
    /// <summary>
    /// No result. The dialog was closed without a specific button being clicked.
    /// </summary>
    None,

    /// <summary>
    /// The user clicked the OK button.
    /// </summary>
    Ok,

    /// <summary>
    /// The user clicked the Cancel button.
    /// </summary>
    Cancel,

    /// <summary>
    /// The user clicked the Yes button.
    /// </summary>
    Yes,

    /// <summary>
    /// The user clicked the No button.
    /// </summary>
    No
}
