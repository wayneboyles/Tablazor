namespace Tablazor.Enums;

/// <summary>
/// Controls where a field label is rendered relative to its input control.
/// </summary>
public enum LabelPosition
{
    /// <summary>
    /// Inherits the form-level <see cref="LabelPosition"/> setting.
    /// Used as the default for <see cref="Attributes.TabFieldAttribute.LabelPosition"/>.
    /// </summary>
    Default,

    /// <summary>
    /// Label appears above the input on its own line — the standard Tabler form layout.
    /// </summary>
    Top,

    /// <summary>
    /// Label appears to the left of the input in a Bootstrap row/col layout (horizontal form).
    /// The label column width is controlled by <see cref="Components.TabDynamicForm{TModel}.LabelColumns"/>.
    /// </summary>
    Left,

    /// <summary>
    /// Floating label: the label starts as a placeholder and animates above the input on focus/fill.
    /// Requires a non-empty placeholder to work correctly with browser autofill.
    /// </summary>
    Floating,

    /// <summary>
    /// No label is rendered. The <c>placeholder</c> attribute (set via
    /// <see cref="Attributes.TabFieldAttribute.Placeholder"/>) serves as the only visual hint.
    /// </summary>
    None
}