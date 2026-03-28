using Tablazor.Enums;

namespace Tablazor.Attributes;

/// <summary>
/// Configures the appearance and behavior of a single property when rendered
/// inside a <c>TabDynamicForm&lt;TModel&gt;</c>.
/// All properties are optional; omitting a property falls back to the form-level default.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class TabFieldAttribute : Attribute
{
    /// <summary>
    /// Overrides the label text.
    /// When <c>null</c>, the label falls back to the property's
    /// <c>[Display(Name = "...")]</c> attribute, then to the property name
    /// converted to title-case words (e.g. <c>BirthDate</c> → <c>Birth Date</c>).
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Overrides the label position for this field only.
    /// <see cref="Enums.LabelPosition.Default"/> inherits from the form-level setting.
    /// </summary>
    public LabelPosition LabelPosition { get; set; } = LabelPosition.Default;

    /// <summary>
    /// Number of Bootstrap grid columns for the label cell in the
    /// <see cref="LabelPosition.Left"/> layout (e.g. <c>3</c> → <c>col-3</c>).
    /// <c>0</c> inherits the form-level <c>LabelColumns</c> setting.
    /// </summary>
    public int LabelColumns { get; set; }

    /// <summary>
    /// Placeholder text rendered inside an empty input.
    /// </summary>
    public string? Placeholder { get; set; }

    /// <summary>
    /// Icon for the left-side input-group prefix.
    /// Accepts either a Tabler CSS class name (e.g. <c>"ti ti-user"</c>) or SVG path data
    /// from <c>TabIcons</c> (e.g. <c>TabIcons.Outline.User</c>).
    /// </summary>
    public string? IconLeft { get; set; }

    /// <summary>
    /// Icon for the right-side input-group suffix.
    /// Accepts either a Tabler CSS class name (e.g. <c>"ti ti-eye"</c>) or SVG path data
    /// from <c>TabIcons</c> (e.g. <c>TabIcons.Outline.Eye</c>).
    /// </summary>
    public string? IconRight { get; set; }

    /// <summary>
    /// Additional CSS class(es) applied to the outermost field wrapper element.
    /// </summary>
    public string? WrapperCssClass { get; set; }

    /// <summary>
    /// Additional CSS class(es) applied directly to the <c>&lt;input&gt;</c> element.
    /// </summary>
    public string? InputCssClass { get; set; }

    /// <summary>
    /// Overrides the auto-detected input type.
    /// <see cref="TabFieldType.Auto"/> detects the type from the CLR property type.
    /// </summary>
    public TabFieldType FieldType { get; set; } = TabFieldType.Auto;

    /// <summary>
    /// The <c>step</c> attribute for <see cref="TabFieldType.Number"/> and
    /// <see cref="TabFieldType.Range"/> inputs (e.g. <c>"0.01"</c>, <c>"5"</c>).
    /// </summary>
    public string? Step { get; set; }

    /// <summary>
    /// Minimum value string for number and range inputs (e.g. <c>"0"</c>).
    /// </summary>
    public string? Min { get; set; }

    /// <summary>Maximum value string for number and range inputs (e.g. <c>"100"</c>).</summary>
    public string? Max { get; set; }

    /// <summary>
    /// Format specifier forwarded to Blazor's <c>InputNumber</c> or <c>InputDate</c>
    /// (e.g. <c>"N2"</c> for 2 decimal places, <c>"yyyy-MM-dd"</c> for dates).
    /// </summary>
    public string? Format { get; set; }

    /// <summary>
    /// Visible row count for <see cref="TabFieldType.Textarea"/> inputs. Defaults to <c>3</c>.
    /// </summary>
    public int Rows { get; set; } = 3;

    /// <summary>
    /// Name of a public property or parameterless method on the model that returns
    /// <c>IEnumerable&lt;SelectOption&gt;</c>.
    /// Used when <see cref="FieldType"/> is <see cref="TabFieldType.Select"/> or
    /// <see cref="TabFieldType.Radio"/> for non-enum property types.
    /// Alternatively, supply options via the form-level
    /// <c>TabDynamicForm.FieldOptions</c> dictionary keyed by property name.
    /// </summary>
    public string? OptionsProvider { get; set; }

    /// <summary>
    /// Rendering order among all fields. Lower values appear first.
    /// Defaults to <see cref="int.MaxValue"/> which preserves declaration order.
    /// </summary>
    public int Order { get; set; } = int.MaxValue;

    /// <summary>
    /// When <c>true</c>, the input is rendered in a read-only state for this field,
    /// regardless of the form-level <c>ReadOnly</c> parameter.
    /// </summary>
    public bool ReadOnly { get; set; }

    /// <summary>
    /// When <c>true</c>, the input is rendered as disabled (greyed-out, not focusable).
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// When <c>true</c>, the field is completely excluded from the rendered form.
    /// </summary>
    public bool Hidden { get; set; }

    /// <summary>
    /// Hint text rendered below the input using Tabler's <c>.form-text</c> style.
    /// </summary>
    public string? HelpText { get; set; }

    /// <summary>
    /// When <c>true</c>, shows a red asterisk (<c>*</c>) next to the label even if the
    /// property has no <c>[Required]</c> attribute.
    /// </summary>
    public bool ShowRequired { get; set; }
}