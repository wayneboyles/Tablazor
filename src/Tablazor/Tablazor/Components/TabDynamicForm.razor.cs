using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

using Tablazor.Attributes;
using Tablazor.Enums;
using Tablazor.Icons;
using Tablazor.Models;

namespace Tablazor.Components;

/// <summary>
/// A highly configurable dynamic form component for Blazor.
/// Reflects on <typeparamref name="TModel"/> at runtime to
/// auto-generate inputs, labels, validation messages and layout — all driven by
/// data-annotation attributes on the model class.
/// </summary>
/// <typeparam name="TModel">The strongly-typed model class backing the form.</typeparam>
public partial class TabDynamicForm<TModel> : TabBaseComponent where TModel : class, new()
{
    private TModel? _lastModel;
    private EditContext? _editContext;
    private List<FormSection>? _sections;
    private readonly Dictionary<string, bool> _sectionCollapsedState = new();
    
    private static readonly HashSet<Type> NumericTypes =
    [
        typeof(byte), typeof(sbyte),
        typeof(short), typeof(ushort),
        typeof(int), typeof(uint),
        typeof(long), typeof(ulong),
        typeof(float), typeof(double),
        typeof(decimal)
    ];
    
    /// <summary>The model instance to bind. Required.</summary>
    [Parameter]
    [EditorRequired]
    public TModel? Model { get; set; }

    /// <summary>
    /// Raised after the model has been mutated by any field change.
    /// </summary>
    [Parameter] 
    public EventCallback<TModel> ModelChanged { get; set; }
    
    /// <summary>
    /// The name of the form
    /// </summary>
    [Parameter]
    public string? FormName { get; set; }
    
    /// <summary>
    /// Raised when the form passes validation on submit.
    /// Receives the current <see cref="EditContext"/>.
    /// </summary>
    [Parameter] 
    public EventCallback<EditContext> OnValidSubmit { get; set; }

    /// <summary>
    /// Raised when the form fails validation on submit.
    /// Receives the current <see cref="EditContext"/>.
    /// </summary>
    [Parameter] 
    public EventCallback<EditContext> OnInvalidSubmit { get; set; }

    /// <summary>
    /// Raised after any individual field value changes.
    /// Receives the property name of the changed field.
    /// </summary>
    [Parameter] 
    public EventCallback<string> OnFieldChanged { get; set; }

    /// <summary>
    /// When <c>true</c> (default), calls <see cref="EditContext.NotifyFieldChanged"/> on
    /// every input event so that per-field validation messages appear immediately.
    /// Set to <c>false</c> to defer validation until form submission.
    /// </summary>
    [Parameter] 
    public bool ValidateOnChange { get; set; } = true;

    /// <summary>When <c>true</c>, all inputs are rendered in a read-only / disabled state.</summary>
    [Parameter] 
    public bool ReadOnly { get; set; }
    
    /// <summary>
    /// Default label position for all fields.
    /// Individual fields can override this via <see cref="TabFieldAttribute.LabelPosition"/>.
    /// Defaults to <see cref="LabelPosition.Top"/>.
    /// </summary>
    [Parameter] 
    public LabelPosition LabelPosition { get; set; } = LabelPosition.Top;

    /// <summary>
    /// Bootstrap column width for the label cell when using
    /// <see cref="LabelPosition.Left"/> layout (e.g. <c>3</c> → <c>col-3</c>).
    /// Defaults to <c>3</c>.
    /// </summary>
    [Parameter] 
    public int LabelColumns { get; set; } = 3;

    /// <summary>
    /// When <c>true</c> (default), wraps the form in a Tabler <c>.card</c> container.
    /// Set to <c>false</c> to render the form fields bare, using your own surrounding markup.
    /// </summary>
    [Parameter] 
    public bool ShowCard { get; set; } = true;

    /// <summary>Card title text displayed in the <c>.card-header</c>.</summary>
    [Parameter] 
    public string? CardTitle { get; set; }

    /// <summary>Card subtitle text displayed beneath the title in muted style.</summary>
    [Parameter] 
    public string? CardSubtitle { get; set; }

    /// <summary>Additional CSS class(es) applied to the <c>.card</c> element.</summary>
    [Parameter] 
    public string? CardCssClass { get; set; }

    /// <summary>Additional CSS class(es) applied to the <c>.card-body</c> element.</summary>
    [Parameter] 
    public string? CardBodyCssClass { get; set; }

    /// <summary>
    /// Custom content rendered inside the card header, replacing the default
    /// title/subtitle rendering.
    /// </summary>
    [Parameter] 
    public RenderFragment? CardHeaderContent { get; set; }

    /// <summary>When <c>true</c> (default), renders a submit button.</summary>
    [Parameter] 
    public bool ShowSubmitButton { get; set; } = true;

    /// <summary>Label text for the submit button. Defaults to <c>"Submit"</c>.</summary>
    [Parameter] 
    public string SubmitButtonText { get; set; } = "Submit";

    /// <summary>Additional CSS class(es) for the submit button. Defaults to <c>"btn-primary"</c>.</summary>
    [Parameter] 
    public string SubmitButtonCssClass { get; set; } = "btn-primary";

    /// <summary>Optional Tabler icon class shown before the submit button text (e.g. <c>"ti ti-device-floppy"</c>).</summary>
    [Parameter] 
    public string? SubmitButtonIcon { get; set; }

    /// <summary>When <c>true</c>, renders a cancel/secondary button beside the submit button.</summary>
    [Parameter] 
    public bool ShowCancelButton { get; set; }

    /// <summary>Label text for the cancel button. Defaults to <c>"Cancel"</c>.</summary>
    [Parameter] 
    public string CancelButtonText { get; set; } = "Cancel";

    /// <summary>Raised when the cancel button is clicked.</summary>
    [Parameter] 
    public EventCallback OnCancel { get; set; }

    /// <summary>
    /// Custom footer content that replaces the default submit/cancel button rendering.
    /// Rendered inside <c>.card-footer</c> when <see cref="ShowCard"/> is <c>true</c>,
    /// or in a plain wrapper <c>div</c> otherwise.
    /// </summary>
    [Parameter] 
    public RenderFragment? FooterContent { get; set; }

    /// <summary>
    /// CSS class applied to the button wrapper <c>div</c> when <see cref="ShowCard"/> is
    /// <c>false</c>. Defaults to <c>"mt-3"</c>.
    /// </summary>
    [Parameter] 
    public string? NoCardFooterCssClass { get; set; }

    /// <summary>Content rendered at the top of the <c>&lt;EditForm&gt;</c>, above all fields.</summary>
    [Parameter] 
    public RenderFragment? FormHeaderContent { get; set; }

    /// <summary>Content rendered at the bottom of the <c>&lt;EditForm&gt;</c>, below all fields.</summary>
    [Parameter] 
    public RenderFragment? FormFooterContent { get; set; }

    /// <summary>Additional CSS class(es) applied to the <c>&lt;EditForm&gt;</c> element.</summary>
    [Parameter] 
    public string? FormCssClass { get; set; }

    /// <summary>CSS class(es) appended to every field wrapper <c>div.mb-3</c>.</summary>
    [Parameter] 
    public string? FieldCssClass { get; set; }

    /// <summary>
    /// A dictionary supplying <see cref="SelectOption"/> lists for non-enum
    /// <see cref="TabFieldType.Select"/> and <see cref="TabFieldType.Radio"/> fields.
    /// The key is the <b>property name</b> (case-sensitive).
    /// For enum properties, options are generated automatically.
    /// </summary>
    [Parameter] 
    public Dictionary<string, IEnumerable<SelectOption>>? FieldOptions { get; set; }
    
    /// <summary>
    /// When <c>true</c>, renders a Blazor <c>&lt;ValidationSummary&gt;</c> at the top of the form.
    /// </summary>
    [Parameter] 
    public bool ShowValidationSummary { get; set; }

    #region Lifecycle

    /// <summary>
    /// Method invoked when the component is ready to start, having received its
    /// initial parameters from its parent in the render tree.
    /// </summary>
    protected override void OnInitialized() => RebuildForm();

    /// <summary>
    /// Method invoked when the component has received parameters from its parent in
    /// the render tree, and the incoming values have been assigned to properties.
    /// </summary>
    protected override void OnParametersSet()
    {
        if (!ReferenceEquals(_lastModel, Model))
        {
            RebuildForm();
        }
    }

    private void RebuildForm()
    {
        if (Model is null)
        {
            return;
        }

        // Detach old subscription
        if (_editContext is not null)
        {
            _editContext.OnValidationStateChanged -= HandleValidationStateChanged;
        }

        _editContext = new EditContext(Model);
        _editContext.OnValidationStateChanged += HandleValidationStateChanged;
        _lastModel = Model;

        var fields = BuildFieldDescriptors();
        _sections = BuildFormSections(fields);

        // Initialize section collapse state (respects InitiallyCollapsed)
        foreach (FormSection sec in _sections.Where(s => s.Collapsible && s.HasTitle))
        {
            _sectionCollapsedState.TryAdd(sec.Title!, sec.InitiallyCollapsed);
        }
    }

    private void HandleValidationStateChanged(object? sender, ValidationStateChangedEventArgs e) => StateHasChanged();

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public override void Dispose()
    {
        if (_editContext is not null)
        {
            _editContext.OnValidationStateChanged -= HandleValidationStateChanged;
        }
    }

    #endregion
    
    #region Form-level RenderFragments

    private RenderFragment RenderFormContent() => builder =>
    {
        var seq = 0;
        
        builder.OpenComponent<EditForm>(seq++);
        builder.AddAttribute(seq++, "EditContext", _editContext);
        builder.AddAttribute(seq++, "OnValidSubmit", EventCallback.Factory.Create<EditContext>(this, HandleValidSubmitInternal));
        builder.AddAttribute(seq++, "OnInvalidSubmit", EventCallback.Factory.Create<EditContext>(this, HandleInvalidSubmitInternal));

        if (!string.IsNullOrWhiteSpace(FormName))
        {
            builder.AddAttribute(seq++, "FormName", FormName);
        }
        
        if (!string.IsNullOrWhiteSpace(FormCssClass))
        {
            builder.AddAttribute(seq++, "class", FormCssClass);
        }

        builder.AddAttribute(seq++, "ChildContent", (RenderFragment<EditContext>)(_ => fb =>
        {
            var fs = 0;
            
            fb.OpenComponent<DataAnnotationsValidator>(fs++);
            fb.CloseComponent();

            if (ShowValidationSummary)
            {
                fb.OpenComponent<ValidationSummary>(fs++);
                fb.CloseComponent();
            }

            if (FormHeaderContent is not null)
            {
                fb.AddContent(fs++, FormHeaderContent);
            }

            foreach (var section in _sections!)
            {
                fb.AddContent(fs++, RenderSection(section));
            }

            if (FormFooterContent is not null)
            {
                fb.AddContent(fs++, FormFooterContent);
            }
        }));

        builder.CloseComponent();
    };

    private RenderFragment RenderActionButtons() => builder =>
    {
        var seq = 0;

        if (ShowSubmitButton)
        {
            builder.OpenElement(seq++, "button");
            builder.AddAttribute(seq++, "type", "submit");
            builder.AddAttribute(seq++, "class", $"btn {SubmitButtonCssClass}");

            if (!string.IsNullOrWhiteSpace(SubmitButtonIcon))
            {
                builder.OpenElement(seq++, "i");
                builder.AddAttribute(seq++, "class", $"{SubmitButtonIcon} me-1");
                builder.CloseElement();
            }

            builder.AddContent(seq++, SubmitButtonText);
            builder.CloseElement();
        }

        if (ShowCancelButton)
        {
            builder.OpenElement(seq++, "button");
            builder.AddAttribute(seq++, "type", "button");
            builder.AddAttribute(seq++, "class", "btn btn-secondary ms-2");
            
            builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, async () =>
            {
                if (OnCancel.HasDelegate) await OnCancel.InvokeAsync();
            }));
            
            builder.AddContent(seq++, CancelButtonText);
            builder.CloseElement();
        }
    };
    
    #endregion

    #region Section / Row rendering

    private RenderFragment RenderSection(FormSection section) => builder =>
    {
        var seq = 0;

        if (section.HasTitle)
        {
            var isCollapsed = IsSectionCollapsed(section);
            var wrapperClass = $"mb-4 {section.CssClass}".Trim();

            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "class", wrapperClass);

            // ── Section header ──────────────────────────────────────────────
            if (section.Collapsible)
            {
                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "class", "d-flex align-items-center mb-3 cursor-pointer user-select-none");
                
                var capturedSection = section;

                builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, () =>
                {
                    _sectionCollapsedState[capturedSection.Title!] = !IsSectionCollapsed(capturedSection);
                    StateHasChanged();
                }));

                builder.OpenElement(seq++, "h5");
                builder.AddAttribute(seq++, "class", "mb-0 me-2");
                builder.AddContent(seq++, section.Title);
                builder.CloseElement();

                builder.OpenElement(seq++, "i");
                builder.AddAttribute(seq++, "class", isCollapsed ? "ti ti-chevron-right text-muted" : "ti ti-chevron-down text-muted");
                builder.CloseElement();

                builder.CloseElement(); // .d-flex
            }
            else
            {
                builder.OpenElement(seq++, "h5");
                builder.AddAttribute(seq++, "class", "mb-1");
                builder.AddContent(seq++, section.Title);
                builder.CloseElement();
            }

            if (!string.IsNullOrWhiteSpace(section.Description))
            {
                builder.OpenElement(seq++, "p");
                builder.AddAttribute(seq++, "class", "text-muted small mb-3");
                builder.AddContent(seq++, section.Description);
                builder.CloseElement();
            }

            // ── Section body ────────────────────────────────────────────────
            if (!section.Collapsible || !isCollapsed)
            {
                foreach (var row in section.Rows)
                {
                    builder.AddContent(seq++, RenderRow(row));
                }
            }

            builder.CloseElement(); // wrapper div
        }
        else
        {
            // No heading — just render rows directly
            foreach (var row in section.Rows)
            {
                builder.AddContent(seq++, RenderRow(row));
            }
        }
    };

    private RenderFragment RenderRow(FormRow row) => builder =>
    {
        if (row.IsMultiColumn)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "row");
            
            var seq = 10;

            foreach (var field in row.Fields)
            {
                var colSpan = field.SameRowAttr?.ColumnSpan ?? 6;
                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "class", $"col-md-{colSpan}");
                builder.AddContent(seq++, RenderFieldWrapper(field));
                builder.CloseElement();
            }

            builder.CloseElement();
        }
        else
        {
            builder.AddContent(0, RenderFieldWrapper(row.Fields[0]));
        }
    };
    
    #endregion

    #region Field wrapper rendering

    private RenderFragment RenderFieldWrapper(FieldDescriptor field) => builder =>
    {
        // Hidden fields: no wrapper at all
        if (field.EffectiveFieldType == TabFieldType.Hidden)
        {
            builder.AddContent(0, RenderInputControl(field));
            return;
        }

        // Checkbox / Toggle have their own special wrapper layout
        if (field.EffectiveFieldType is TabFieldType.Checkbox or TabFieldType.Toggle)
        {
            builder.AddContent(0, RenderCheckboxWrapper(field));
            return;
        }

        var messages = GetValidationMessages(field);
        var enumerable = messages as string[] ?? messages.ToArray();
        var hasErrors = enumerable.Any();

        switch (field.EffectiveLabelPosition)
        {
            case LabelPosition.Left:
                RenderLeftLabelWrapper(builder, field, hasErrors, enumerable);
                break;

            case LabelPosition.Floating:
                RenderFloatingLabelWrapper(builder, field, hasErrors, enumerable);
                break;

            default: // Top or None
                RenderTopOrNoneLabelWrapper(builder, field, hasErrors, enumerable);
                break;
        }
    };

    private void RenderTopOrNoneLabelWrapper(RenderTreeBuilder builder, FieldDescriptor field, bool hasErrors, IEnumerable<string> messages)
    {
        var seq = 0;
        var wrapperCss = BuildWrapperCssClass(field);

        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", wrapperCss);

        // Label
        if (field.EffectiveLabelPosition != LabelPosition.None)
        {
            builder.OpenElement(seq++, "label");
            builder.AddAttribute(seq++, "class", "form-label");
            builder.AddAttribute(seq++, "for", field.FieldId);
            builder.AddContent(seq++, field.Label);
            
            if (field.IsRequired)
            {
                RenderRequiredStar(builder, ref seq);
            }

            builder.CloseElement();
        }

        // Input (possibly with icon addons)
        builder.AddContent(seq++, WrapWithIconAddon(field, hasErrors));

        // Help text
        RenderHelpText(builder, field, ref seq);

        // Validation messages
        RenderValidationMessages(builder, messages, ref seq);

        builder.CloseElement();
    }

    private void RenderLeftLabelWrapper(RenderTreeBuilder builder, FieldDescriptor field, bool hasErrors, IEnumerable<string> messages)
    {
        var seq = 0;
        var labelCols = field.FieldAttr?.LabelColumns > 0 ? field.FieldAttr.LabelColumns : LabelColumns;
        var wrapperCss = $"mb-3 row align-items-center {field.FieldAttr?.WrapperCssClass} {FieldCssClass}".Trim();

        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", wrapperCss);

        // Label column
        builder.OpenElement(seq++, "label");
        builder.AddAttribute(seq++, "class", $"col-{labelCols} col-form-label");
        builder.AddAttribute(seq++, "for", field.FieldId);
        builder.AddContent(seq++, field.Label);
        
        if (field.IsRequired)
        {
            RenderRequiredStar(builder, ref seq);
        }

        builder.CloseElement();

        // Input column
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "col");
        builder.AddContent(seq++, WrapWithIconAddon(field, hasErrors));
        RenderHelpText(builder, field, ref seq);
        RenderValidationMessages(builder, messages, ref seq);
        builder.CloseElement();

        builder.CloseElement();
    }

    private void RenderFloatingLabelWrapper(RenderTreeBuilder builder, FieldDescriptor field, bool hasErrors, IEnumerable<string> messages)
    {
        var seq = 0;
        var wrapperCss = $"form-floating mb-3 {field.FieldAttr?.WrapperCssClass} {FieldCssClass}".Trim();

        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", wrapperCss);

        // Input FIRST (CSS requires it for floating labels to work)
        builder.AddContent(seq++, RenderInputControl(field));

        // Label AFTER input
        builder.OpenElement(seq++, "label");
        builder.AddAttribute(seq++, "for", field.FieldId);
        builder.AddContent(seq++, field.Label);
        
        if (field.IsRequired)
        {
            RenderRequiredStar(builder, ref seq);
        }

        builder.CloseElement();

        RenderValidationMessages(builder, messages, ref seq);

        builder.CloseElement();
    }

    private RenderFragment RenderCheckboxWrapper(FieldDescriptor field) => builder =>
    {
        var seq = 0;
        var messages = GetValidationMessages(field);
        var enumerable = messages as string[] ?? messages.ToArray();
        var hasErrors = enumerable.Any();
        var wrapperCss = BuildWrapperCssClass(field);
        var capturedProp = field.Property;
        var isDisabled = IsFieldDisabled(field);
        var isToggle = field.EffectiveFieldType == TabFieldType.Toggle;

        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", wrapperCss);

        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", isToggle ? "form-check form-switch" : "form-check");

        // Input
        builder.OpenElement(seq++, "input");
        builder.AddAttribute(seq++, "type", "checkbox");
        builder.AddAttribute(seq++, "class", $"form-check-input{(hasErrors ? " is-invalid" : "")}");
        builder.AddAttribute(seq++, "id", field.FieldId);
        
        if (isToggle)
        {
            builder.AddAttribute(seq++, "role", "switch");
        }

        builder.AddAttribute(seq++, "checked", (bool)(capturedProp.GetValue(Model) ?? false));
        
        if (isDisabled)
        {
            builder.AddAttribute(seq++, "disabled", true);
        }

        builder.AddAttribute(seq++, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, e =>
        {
            capturedProp.SetValue(Model, (bool)(e.Value ?? false));
            NotifyFieldChange(field);
        }));
        
        builder.CloseElement(); // input

        // Label
        builder.OpenElement(seq++, "label");
        builder.AddAttribute(seq++, "class", "form-check-label");
        builder.AddAttribute(seq++, "for", field.FieldId);
        builder.AddContent(seq++, field.Label);
        
        if (field.IsRequired)
        {
            RenderRequiredStar(builder, ref seq);
        }

        builder.CloseElement(); // label

        builder.CloseElement(); // form-check [form-switch]

        RenderHelpText(builder, field, ref seq);
        RenderValidationMessages(builder, enumerable, ref seq);

        builder.CloseElement(); // mb-3 wrapper
    };

    #endregion
    
    #region Input Control Rendering

    private RenderFragment WrapWithIconAddon(FieldDescriptor field, bool hasErrors) => builder =>
    {
        if (!field.HasIconLeft && !field.HasIconRight)
        {
            builder.AddContent(0, RenderInputControl(field));
            return;
        }

        var seq = 0;
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "input-group");

        if (field.HasIconLeft)
        {
            builder.OpenElement(seq++, "span");
            builder.AddAttribute(seq++, "class", "input-group-text");
            RenderIcon(builder, field.FieldAttr!.IconLeft, ref seq);
            builder.CloseElement();
        }

        builder.AddContent(seq++, RenderInputControl(field));

        if (field.HasIconRight)
        {
            builder.OpenElement(seq++, "span");
            builder.AddAttribute(seq++, "class", "input-group-text");
            RenderIcon(builder, field.FieldAttr!.IconRight, ref seq);
            builder.CloseElement();
        }

        builder.CloseElement(); // .input-group
    };

    private RenderFragment RenderInputControl(FieldDescriptor field) => builder =>
    {
        var isDisabled = IsFieldDisabled(field);
        var isReadOnly = ReadOnly || field.FieldAttr?.ReadOnly == true;
        var hasErrors = GetValidationMessages(field).Any();
        var inputCss = BuildInputCssClass(field, hasErrors);
        var placeholder = ResolveFloatingPlaceholder(field);
        var prop = field.Property;
        var seq = 0;

        switch (field.EffectiveFieldType)
        {
            case TabFieldType.Text:
            case TabFieldType.Password:
            case TabFieldType.Email:
            case TabFieldType.Tel:
            case TabFieldType.Url:
            case TabFieldType.Search:
            case TabFieldType.Color:
                {
                    var htmlType = field.EffectiveFieldType switch
                    {
                        TabFieldType.Password => "password",
                        TabFieldType.Email => "email",
                        TabFieldType.Tel => "tel",
                        TabFieldType.Url => "url",
                        TabFieldType.Search => "search",
                        TabFieldType.Color => "color",
                        _ => "text"
                    };
                    
                    var capturedProp = prop;
                    builder.OpenElement(seq++, "input");
                    builder.AddAttribute(seq++, "type", htmlType);
                    builder.AddAttribute(seq++, "class", inputCss);
                    builder.AddAttribute(seq++, "id", field.FieldId);
                    builder.AddAttribute(seq++, "placeholder", placeholder);
                    builder.AddAttribute(seq++, "value", (string?)capturedProp.GetValue(Model));
                    
                    if (isDisabled)
                    {
                        builder.AddAttribute(seq++, "disabled", true);
                    }

                    if (isReadOnly)
                    {
                        builder.AddAttribute(seq++, "readonly", true);
                    }

                    builder.AddAttribute(seq++, "oninput", EventCallback.Factory.Create<ChangeEventArgs>(this, e =>
                    {
                        capturedProp.SetValue(Model, e.Value?.ToString());
                        NotifyFieldChange(field);
                    }));
                    
                    builder.SetUpdatesAttributeName("value");
                    builder.CloseElement();
                    break;
                }

            // ── Textarea ──────────────────────────────────────────────────────

            case TabFieldType.Textarea:
                {
                    var rows = field.FieldAttr?.Rows ?? 3;
                    var capturedProp = prop;
                    builder.OpenElement(seq++, "textarea");
                    builder.AddAttribute(seq++, "class", inputCss);
                    builder.AddAttribute(seq++, "id", field.FieldId);
                    builder.AddAttribute(seq++, "placeholder", placeholder);
                    builder.AddAttribute(seq++, "rows", rows.ToString());
                    
                    if (isDisabled)
                    {
                        builder.AddAttribute(seq++, "disabled", true);
                    }

                    if (isReadOnly)
                    {
                        builder.AddAttribute(seq++, "readonly", true);
                    }

                    builder.AddAttribute(seq++, "oninput", EventCallback.Factory.Create<ChangeEventArgs>(this, e =>
                    {
                        capturedProp.SetValue(Model, e.Value?.ToString());
                        NotifyFieldChange(field);
                    }));
                    
                    builder.AddContent(seq++, (string?)capturedProp.GetValue(Model) ?? string.Empty);
                    builder.CloseElement();
                    
                    break;
                }

            // ── Number (InputNumber<T>) ───────────────────────────────────────

            case TabFieldType.Number:
                {
                    var numType = prop.PropertyType;
                    var compType = typeof(InputNumber<>).MakeGenericType(numType);
                    var capturedProp = prop;

                    builder.OpenComponent(seq++, compType);
                    builder.AddAttribute(seq++, "class", inputCss);
                    builder.AddAttribute(seq++, "id", field.FieldId);
                    builder.AddAttribute(seq++, "placeholder", placeholder);
                    builder.AddAttribute(seq++, "Value", capturedProp.GetValue(Model));
                    builder.AddAttribute(seq++, "ValueChanged", CreateValueChangedCallback(capturedProp, numType, field));
                    builder.AddAttribute(seq++, "ValueExpression", CreateExpressionDynamic(capturedProp, numType));
                    
                    if (!string.IsNullOrWhiteSpace(field.FieldAttr?.Step))
                    {
                        builder.AddAttribute(seq++, "step", field.FieldAttr.Step);
                    }

                    if (!string.IsNullOrWhiteSpace(field.FieldAttr?.Min))
                    {
                        builder.AddAttribute(seq++, "min", field.FieldAttr.Min);
                    }

                    if (!string.IsNullOrWhiteSpace(field.FieldAttr?.Max))
                    {
                        builder.AddAttribute(seq++, "max", field.FieldAttr.Max);
                    }

                    if (!string.IsNullOrWhiteSpace(field.FieldAttr?.Format))
                    {
                        builder.AddAttribute(seq++, "ParsingErrorMessage",
                            $"The {field.Label} field must be a valid number.");
                    }

                    if (isDisabled)
                    {
                        builder.AddAttribute(seq++, "disabled", true);
                    }

                    if (isReadOnly)
                    {
                        builder.AddAttribute(seq++, "readonly", true);
                    }

                    builder.CloseComponent();
                    break;
                }

            // ── Date / Time (InputDate<T>) ────────────────────────────────────

            case TabFieldType.Date:
            case TabFieldType.Time:
            case TabFieldType.DateTimeLocal:
                {
                    var dateType = prop.PropertyType;
                    var compType = typeof(InputDate<>).MakeGenericType(dateType);
                    
                    var dateInputType = field.EffectiveFieldType switch
                    {
                        TabFieldType.Time          => InputDateType.Time,
                        TabFieldType.DateTimeLocal => InputDateType.DateTimeLocal,
                        _                          => InputDateType.Date
                    };
                    
                    var capturedProp = prop;

                    builder.OpenComponent(seq++, compType);
                    builder.AddAttribute(seq++, "class", inputCss);
                    builder.AddAttribute(seq++, "id", field.FieldId);
                    builder.AddAttribute(seq++, "Type", dateInputType);
                    builder.AddAttribute(seq++, "Value", capturedProp.GetValue(Model));
                    builder.AddAttribute(seq++, "ValueChanged", CreateValueChangedCallback(capturedProp, dateType, field));
                    builder.AddAttribute(seq++, "ValueExpression", CreateExpressionDynamic(capturedProp, dateType));
                    
                    if (!string.IsNullOrWhiteSpace(field.FieldAttr?.Format))
                    {
                        builder.AddAttribute(seq++, "DisplayName", field.Label);
                    }

                    if (isDisabled)
                    {
                        builder.AddAttribute(seq++, "disabled", true);
                    }

                    builder.CloseComponent();
                    break;
                }

            // ── Select ────────────────────────────────────────────────────────

            case TabFieldType.Select:
                {
                    if (field.UnderlyingType.IsEnum)
                    {
                        RenderEnumSelect(builder, field, inputCss, isDisabled, ref seq);
                    }
                    else
                    {
                        RenderOptionSelect(builder, field, inputCss, isDisabled, ref seq);
                    }

                    break;
                }

            // ── Radio group ───────────────────────────────────────────────────

            case TabFieldType.Radio:
                RenderRadioGroup(builder, field, isDisabled, ref seq);
                break;

            // ── Range slider ──────────────────────────────────────────────────

            case TabFieldType.Range:
                {
                    var capturedProp = prop;
                    builder.OpenElement(seq++, "input");
                    builder.AddAttribute(seq++, "type", "range");
                    builder.AddAttribute(seq++, "class", $"form-range{(hasErrors ? " is-invalid" : "")} {field.FieldAttr?.InputCssClass}".Trim());
                    builder.AddAttribute(seq++, "id", field.FieldId);
                    builder.AddAttribute(seq++, "value", capturedProp.GetValue(Model)?.ToString() ?? "0");
                    
                    if (!string.IsNullOrWhiteSpace(field.FieldAttr?.Min))
                    {
                        builder.AddAttribute(seq++, "min", field.FieldAttr.Min);
                    }

                    if (!string.IsNullOrWhiteSpace(field.FieldAttr?.Max))
                    {
                        builder.AddAttribute(seq++, "max", field.FieldAttr.Max);
                    }

                    if (!string.IsNullOrWhiteSpace(field.FieldAttr?.Step))
                    {
                        builder.AddAttribute(seq++, "step", field.FieldAttr.Step);
                    }

                    if (isDisabled)
                    {
                        builder.AddAttribute(seq++, "disabled", true);
                    }

                    builder.AddAttribute(seq++, "oninput", EventCallback.Factory.Create<ChangeEventArgs>(this, e =>
                    {
                        TrySetConvertedValue(capturedProp, field, e.Value?.ToString());
                        NotifyFieldChange(field);
                    }));
                    
                    builder.SetUpdatesAttributeName("value");
                    builder.CloseElement();
                    break;
                }

            // ── File ──────────────────────────────────────────────────────────

            case TabFieldType.File:
                {
                    builder.OpenElement(seq++, "input");
                    builder.AddAttribute(seq++, "type", "file");
                    builder.AddAttribute(seq++, "class", $"form-control{(hasErrors ? " is-invalid" : "")} {field.FieldAttr?.InputCssClass}".Trim());
                    builder.AddAttribute(seq++, "id", field.FieldId);
                    
                    if (isDisabled)
                    {
                        builder.AddAttribute(seq++, "disabled", true);
                    }

                    builder.CloseElement();
                    break;
                }

            // ── Hidden ────────────────────────────────────────────────────────

            case TabFieldType.Hidden:
                {
                    var capturedProp = prop;
                    builder.OpenElement(seq++, "input");
                    builder.AddAttribute(seq++, "type", "hidden");
                    builder.AddAttribute(seq++, "id", field.FieldId);
                    builder.AddAttribute(seq++, "value", capturedProp.GetValue(Model)?.ToString() ?? string.Empty);
                    builder.CloseElement();
                    break;
                }
        }
    };

    // ── Enum <select> ─────────────────────────────────────────────────────────

    private void RenderEnumSelect(RenderTreeBuilder builder, FieldDescriptor field, string css, bool disabled, ref int seq)
    {
        var propType = field.Property.PropertyType;
        var capturedProp = field.Property;
        var compType = typeof(InputSelect<>).MakeGenericType(propType);

        builder.OpenComponent(seq++, compType);
        builder.AddAttribute(seq++, "class", css);
        builder.AddAttribute(seq++, "id", field.FieldId);
        builder.AddAttribute(seq++, "Value", capturedProp.GetValue(Model));
        builder.AddAttribute(seq++, "ValueChanged", CreateValueChangedCallback(capturedProp, propType, field));
        builder.AddAttribute(seq++, "ValueExpression", CreateExpressionDynamic(capturedProp, propType));
        
        if (disabled)
        {
            builder.AddAttribute(seq++, "disabled", true);
        }

        builder.AddAttribute(seq++, "ChildContent", (RenderFragment)(ob =>
        {
            var os = 0;
            
            if (field.IsNullable)
            {
                ob.OpenElement(os++, "option");
                ob.AddAttribute(os++, "value", string.Empty);
                ob.AddContent(os++, "— Select —");
                ob.CloseElement();
            }

            foreach (var enumVal in Enum.GetValues(field.UnderlyingType))
            {
                var memberInfo = field.UnderlyingType.GetMember(enumVal.ToString()!).FirstOrDefault();
                var displayAttr = memberInfo?.GetCustomAttribute<DisplayAttribute>();
                var text = displayAttr?.Name ?? ToTitleCase(enumVal.ToString()!);

                ob.OpenElement(os++, "option");
                ob.AddAttribute(os++, "value", enumVal.ToString());
                ob.AddContent(os++, text);
                ob.CloseElement();
            }
        }));

        builder.CloseComponent();
    }

    // ── Non-enum <select> (with SelectOption list) ────────────────────────────

    private void RenderOptionSelect(RenderTreeBuilder builder, FieldDescriptor field, string css, bool disabled, ref int seq)
    {
        var propType = field.Property.PropertyType;
        var capturedProp = field.Property;
        var options = GetFieldOptions(field);
        var compType = typeof(InputSelect<>).MakeGenericType(propType);

        builder.OpenComponent(seq++, compType);
        builder.AddAttribute(seq++, "class", css);
        builder.AddAttribute(seq++, "id", field.FieldId);
        builder.AddAttribute(seq++, "Value", capturedProp.GetValue(Model));
        builder.AddAttribute(seq++, "ValueChanged", CreateValueChangedCallback(capturedProp, propType, field));
        builder.AddAttribute(seq++, "ValueExpression", CreateExpressionDynamic(capturedProp, propType));
        
        if (disabled)
        {
            builder.AddAttribute(seq++, "disabled", true);
        }

        builder.AddAttribute(seq++, "ChildContent", (RenderFragment)(ob =>
        {
            var os = 0;
            ob.OpenElement(os++, "option");
            ob.AddAttribute(os++, "value", string.Empty);
            ob.AddContent(os++, "— Select —");
            ob.CloseElement();

            if (options is null)
            {
                return;
            }

            string? lastGroup = null;
            foreach (var opt in options)
            {
                if (opt.Group != lastGroup)
                {
                    if (lastGroup is not null)
                    {
                        ob.CloseElement(); // close optgroup
                    }

                    if (opt.Group is not null)
                    {
                        ob.OpenElement(os++, "optgroup");
                        ob.AddAttribute(os++, "label", opt.Group);
                    }
                    
                    lastGroup = opt.Group;
                }

                ob.OpenElement(os++, "option");
                ob.AddAttribute(os++, "value", opt.Value);
                
                if (opt.Disabled)
                {
                    ob.AddAttribute(os++, "disabled", true);
                }

                ob.AddContent(os++, opt.Text);
                ob.CloseElement();
            }

            if (lastGroup is not null)
            {
                ob.CloseElement(); // close last optgroup
            }
        }));

        builder.CloseComponent();
    }

    // ── Radio group ───────────────────────────────────────────────────────────

    private void RenderRadioGroup(RenderTreeBuilder builder, FieldDescriptor field, bool disabled, ref int seq)
    {
        var options = field.UnderlyingType.IsEnum
            ? GetEnumAsOptions(field.UnderlyingType)
            : GetFieldOptions(field) ?? [];

        var capturedProp = field.Property;
        var currentValue = capturedProp.GetValue(Model)?.ToString() ?? string.Empty;
        var hasErrors = GetValidationMessages(field).Any();

        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "row g-2");

        foreach (var opt in options)
        {
            var capturedValue = opt.Value;
            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "class", "col-auto");

            builder.OpenElement(seq++, "label");
            builder.AddAttribute(seq++, "class", "form-check");

            builder.OpenElement(seq++, "input");
            builder.AddAttribute(seq++, "type", "radio");
            builder.AddAttribute(seq++, "class", $"form-check-input{(hasErrors ? " is-invalid" : "")}");
            builder.AddAttribute(seq++, "name", field.FieldId);
            builder.AddAttribute(seq++, "value", capturedValue);
            
            if (currentValue == capturedValue)
            {
                builder.AddAttribute(seq++, "checked", true);
            }

            if (disabled || opt.Disabled)
            {
                builder.AddAttribute(seq++, "disabled", true);
            }

            builder.AddAttribute(seq++, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, _ =>
            {
                TrySetConvertedValue(capturedProp, field, capturedValue);
                NotifyFieldChange(field);
            }));
            
            builder.CloseElement(); // input

            builder.OpenElement(seq++, "span");
            builder.AddAttribute(seq++, "class", "form-check-label");
            builder.AddContent(seq++, opt.Text);
            builder.CloseElement(); // span

            builder.CloseElement(); // label
            builder.CloseElement(); // col-auto
        }

        builder.CloseElement(); // row
    }

    #endregion

    #region Render Helpers

    private static void RenderRequiredStar(RenderTreeBuilder builder, ref int seq)
    {
        builder.OpenElement(seq++, "span");
        builder.AddAttribute(seq++, "class", "text-danger ms-1");
        builder.AddAttribute(seq++, "aria-hidden", "true");
        builder.AddContent(seq++, "*");
        builder.CloseElement();
    }

    private static void RenderHelpText(RenderTreeBuilder builder, FieldDescriptor field, ref int seq)
    {
        if (string.IsNullOrWhiteSpace(field.FieldAttr?.HelpText)) return;
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "form-text text-muted");
        builder.AddContent(seq++, field.FieldAttr.HelpText);
        builder.CloseElement();
    }

    private static void RenderValidationMessages(RenderTreeBuilder builder, IEnumerable<string> messages, ref int seq)
    {
        foreach (var msg in messages)
        {
            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "class", "invalid-feedback d-block");
            builder.AddContent(seq++, msg);
            builder.CloseElement();
        }
    }

    #endregion

    #region Reflection — Field descriptors & form structure

    private List<FieldDescriptor> BuildFieldDescriptors()
    {
        var props = typeof(TModel)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.CanWrite)
            .ToList();

        var result = new List<FieldDescriptor>();
        var declaredOrder = 0;

        foreach (var prop in props)
        {
            var fieldAttr   = prop.GetCustomAttribute<TabFieldAttribute>();
            if (fieldAttr?.Hidden == true) continue;

            var sameRowAttr = prop.GetCustomAttribute<SameRowAttribute>();
            var sectionAttr = prop.GetCustomAttribute<TabSectionAttribute>();
            var displayAttr = prop.GetCustomAttribute<DisplayAttribute>();
            var requiredAttr = prop.GetCustomAttribute<RequiredAttribute>();

            var propType       = prop.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(propType) ?? propType;
            var isNullable     = Nullable.GetUnderlyingType(propType) is not null;

            var label = fieldAttr?.Label
                ?? displayAttr?.GetName()
                ?? ToTitleCase(prop.Name);

            var order = (fieldAttr?.Order is int o && o != int.MaxValue)
                ? o
                : declaredOrder;

            var fieldType = DetermineFieldType(fieldAttr, underlyingType);

            var labelPos = (fieldAttr?.LabelPosition ?? LabelPosition.Default) == LabelPosition.Default
                ? LabelPosition
                : fieldAttr!.LabelPosition;

            result.Add(new FieldDescriptor
            {
                Property = prop,
                FieldAttr = fieldAttr,
                SameRowAttr = sameRowAttr,
                SectionAttr = sectionAttr,
                Label = label,
                EffectiveLabelPosition = labelPos,
                EffectiveFieldType = fieldType,
                IsRequired = requiredAttr is not null || fieldAttr?.ShowRequired == true,
                IsNullable = isNullable,
                UnderlyingType = underlyingType,
                Order = order
            });

            declaredOrder++;
        }

        return result;
    }

    private static TabFieldType DetermineFieldType(TabFieldAttribute? attr, Type underlyingType)
    {
        if (attr?.FieldType is { } ft && ft != TabFieldType.Auto)
        {
            return ft;
        }

        if (underlyingType == typeof(string))
        {
            return TabFieldType.Text;
        }

        if (underlyingType == typeof(bool))
        {
            return TabFieldType.Checkbox;
        }

        if (underlyingType == typeof(DateTime) || underlyingType == typeof(DateTimeOffset))
        {
            return TabFieldType.DateTimeLocal;
        }

        if (underlyingType == typeof(DateOnly))
        {
            return TabFieldType.Date;
        }

        if (underlyingType == typeof(TimeOnly))
        {
            return TabFieldType.Time;
        }

        if (underlyingType.IsEnum)
        {
            return TabFieldType.Select;
        }

        if (IsNumericType(underlyingType))
        {
            return TabFieldType.Number;
        }

        return TabFieldType.Text; // safe fallback
    }

    private List<FormSection> BuildFormSections(List<FieldDescriptor> fields)
    {
        var sections = new List<FormSection>();
        var rows     = BuildFormRows(fields.OrderBy(f => f.Order).ToList());

        FormSection? current = null;

        foreach (var row in rows)
        {
            var sectionAttr = row.Fields.FirstOrDefault(f => f.SectionAttr is not null)?.SectionAttr;

            if (sectionAttr is not null)
            {
                current = new FormSection
                {
                    Title              = sectionAttr.Title,
                    Description        = sectionAttr.Description,
                    Collapsible        = sectionAttr.Collapsible,
                    InitiallyCollapsed = sectionAttr.InitiallyCollapsed,
                    CssClass           = sectionAttr.CssClass
                };
                sections.Add(current);
            }
            else if (current is null)
            {
                current = new FormSection();
                sections.Add(current);
            }

            current.Rows.Add(row);
        }

        return sections;
    }

    private static List<FormRow> BuildFormRows(List<FieldDescriptor> orderedFields)
    {
        var rows            = new List<FormRow>();
        var processedRowIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var field in orderedFields)
        {
            if (field.SameRowAttr is { } sra)
            {
                if (!processedRowIds.Add(sra.RowId)) continue;  // already grouped

                var grouped = orderedFields
                    .Where(f => string.Equals(
                        f.SameRowAttr?.RowId, sra.RowId, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(f => f.SameRowAttr!.Order)
                    .ToList();

                rows.Add(new FormRow { RowId = sra.RowId, Fields = grouped });
            }
            else
            {
                rows.Add(new FormRow { Fields = [field] });
            }
        }

        return rows;
    }

    #endregion

    #region Expression & EventCallback factories

    /// <summary>
    /// Creates a typed <c>EventCallback&lt;T&gt;</c> at runtime, where <c>T</c> is
    /// <paramref name="valueType"/>, and dispatches the value back to the model property.
    /// </summary>
    private object CreateValueChangedCallback(PropertyInfo prop, Type valueType, FieldDescriptor field)
    {
        // Dispatch to the generic helper via reflection to get the right EventCallback<T>
        return GetType()
            .GetMethod(nameof(CreateTypedCallback), BindingFlags.NonPublic | BindingFlags.Instance)!
            .MakeGenericMethod(valueType)
            .Invoke(this, [prop, field])!;
    }

    private EventCallback<T> CreateTypedCallback<T>(PropertyInfo prop, FieldDescriptor field)
    {
        return EventCallback.Factory.Create<T>(this, value =>
        {
            prop.SetValue(Model, value);
            NotifyFieldChange(field);
        });
    }

    /// <summary>
    /// Creates an <c>Expression&lt;Func&lt;T&gt;&gt;</c> over the model property so that
    /// Blazor input components can track the field for validation purposes.
    /// </summary>
    private object CreateExpressionDynamic(PropertyInfo prop, Type valueType)
    {
        return GetType()
            .GetMethod(nameof(CreateTypedExpression), BindingFlags.NonPublic | BindingFlags.Instance)!
            .MakeGenericMethod(valueType)
            .Invoke(this, [prop])!;
    }

    private Expression<Func<T>> CreateTypedExpression<T>(PropertyInfo prop)
    {
        var modelConst = Expression.Constant(Model, typeof(TModel));
        var propAccess = Expression.Property(modelConst, prop);
        return Expression.Lambda<Func<T>>(propAccess);
    }

    #endregion

    #region Validation Helpers

    private void NotifyFieldChange(FieldDescriptor field)
    {
        if (ModelChanged.HasDelegate)
        {
            _ = ModelChanged.InvokeAsync(Model);
        }

        if (ValidateOnChange && _editContext is not null && Model is not null)
        {
            _editContext.NotifyFieldChanged(new FieldIdentifier(Model, field.Property.Name));
        }

        if (OnFieldChanged.HasDelegate)
        {
            _ = OnFieldChanged.InvokeAsync(field.Property.Name);
        }
    }

    private IEnumerable<string> GetValidationMessages(FieldDescriptor field)
    {
        if (_editContext is null || Model is null)
        {
            return [];
        }

        return _editContext.GetValidationMessages(new FieldIdentifier(Model, field.Property.Name));
    }

    private async Task HandleValidSubmitInternal(EditContext ctx)
    {
        if (OnValidSubmit.HasDelegate)
        {
            await OnValidSubmit.InvokeAsync(ctx);
        }
    }

    private async Task HandleInvalidSubmitInternal(EditContext ctx)
    {
        if (OnInvalidSubmit.HasDelegate)
        {
            await OnInvalidSubmit.InvokeAsync(ctx);
        }
    }

    #endregion

    #region Enum Helpers

    private IEnumerable<SelectOption>? GetFieldOptions(FieldDescriptor field)
    {
        // 1. Form-level FieldOptions dictionary
        if (FieldOptions?.TryGetValue(field.Property.Name, out var opts) == true)
        {
            return opts;
        }

        // 2. OptionsProvider on the field attribute (property or method on the model)
        var providerName = field.FieldAttr?.OptionsProvider;
        if (string.IsNullOrWhiteSpace(providerName))
        {
            return null;
        }

        var provProp = typeof(TModel).GetProperty(providerName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        if (provProp is not null)
        {
            var isStatic = provProp.GetMethod?.IsStatic == true;
            return (provProp.GetValue(isStatic ? null : Model) as IEnumerable<SelectOption>);
        }

        var provMethod = typeof(TModel).GetMethod(providerName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static, binder: null, types: [], modifiers: null);
        if (provMethod is not null)
        {
            return (provMethod.Invoke(provMethod.IsStatic ? null : Model, null) as IEnumerable<SelectOption>);
        }

        return null;
    }

    private static List<SelectOption> GetEnumAsOptions(Type enumType)
    {
        return Enum.GetValues(enumType)
            .Cast<object>()
            .Select(val =>
            {
                var mi = enumType.GetMember(val.ToString()!).FirstOrDefault();
                var display = mi?.GetCustomAttribute<DisplayAttribute>();
                var text = display?.GetName() ?? ToTitleCase(val.ToString()!);
                return new SelectOption(val.ToString()!, text);
            })
            .ToList();
    }

    #endregion

    #region CSS Class Helpers

    private string BuildInputCssClass(FieldDescriptor field, bool hasErrors)
    {
        // Select fields use .form-select; everything else uses .form-control
        var baseClass = field.EffectiveFieldType == TabFieldType.Select
            ? "form-select"
            : "form-control";

        return string
            .Join(" ", new[] { baseClass, hasErrors ? "is-invalid" : null, field.FieldAttr?.InputCssClass }
            .Where(c => !string.IsNullOrWhiteSpace(c))!);
    }

    private string BuildWrapperCssClass(FieldDescriptor field)
    {
        return string
            .Join(" ", new[] { "mb-3", field.FieldAttr?.WrapperCssClass, FieldCssClass }
            .Where(c => !string.IsNullOrWhiteSpace(c))!);
    }

    #endregion

    #region Misc Helpers

    private bool IsFieldDisabled(FieldDescriptor field) => ReadOnly || field.FieldAttr?.ReadOnly == true || field.FieldAttr?.Disabled == true;

    private bool IsSectionCollapsed(FormSection section)
    {
        if (!section.Collapsible || !section.HasTitle)
        {
            return false;
        }

        return _sectionCollapsedState.TryGetValue(section.Title!, out var v) ? v : section.InitiallyCollapsed;
    }

    /// <summary>
    /// For floating-label inputs the placeholder must be non-empty; fall back to the
    /// label text so the CSS animation works correctly.
    /// </summary>
    private string ResolveFloatingPlaceholder(FieldDescriptor field)
    {
        if (!string.IsNullOrWhiteSpace(field.FieldAttr?.Placeholder))
        {
            return field.FieldAttr.Placeholder;
        }

        return field.EffectiveLabelPosition == LabelPosition.Floating
            ? field.Label   // needed for floating CSS to function
            : string.Empty;
    }

    private void TrySetConvertedValue(PropertyInfo prop, FieldDescriptor field, string? rawValue)
    {
        try
        {
            if (string.IsNullOrEmpty(rawValue) && field.IsNullable)
            {
                prop.SetValue(Model, null);
                return;
            }

            object? converted;
            if (field.UnderlyingType.IsEnum)
                converted = Enum.Parse(field.UnderlyingType, rawValue!);
            else
                converted = Convert.ChangeType(rawValue, field.UnderlyingType);

            prop.SetValue(Model, converted);
        }
        catch
        {
            // Silently ignore unconvertible values — validation will surface the issue
        }
    }

    private static bool IsNumericType(Type t) => NumericTypes.Contains(t);

    /// <summary>
    /// Returns true when <paramref name="value"/> is SVG path data (e.g. from <see cref="TabIcons"/>)
    /// rather than a CSS class name like <c>"ti ti-user"</c>.
    /// SVG paths always start with the M/m command followed by a coordinate.
    /// </summary>
    private static bool IsSvgPathData(string? value)
        => !string.IsNullOrEmpty(value)
           && value.Length > 1
           && (value[0] == 'M' || value[0] == 'm')
           && (char.IsDigit(value[1]) || value[1] == ' ' || value[1] == '-');

    /// <summary>
    /// Renders an icon into the builder. Accepts either a CSS class name (e.g. <c>"ti ti-user"</c>)
    /// or SVG path data from <see cref="TabIcons"/> (e.g. <c>TabIcons.Outline.User</c>).
    /// </summary>
    private static void RenderIcon(RenderTreeBuilder builder, string? icon, ref int seq)
    {
        if (string.IsNullOrEmpty(icon)) return;

        if (IsSvgPathData(icon))
            builder.AddMarkupContent(seq++, TabIconsRenderer.Render(icon));
        else
        {
            builder.OpenElement(seq++, "i");
            builder.AddAttribute(seq++, "class", icon);
            builder.CloseElement();
        }
    }

    /// <summary>
    /// Converts a camelCase or PascalCase identifier to space-separated title-case words.
    /// e.g. <c>firstName</c> → <c>First Name</c>, <c>HTMLParser</c> → <c>HTML Parser</c>.
    /// </summary>
    private static string ToTitleCase(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;
        
        // Insert space before: uppercase that follows lowercase, or uppercase that
        // precedes a lowercase (handles acronyms like "HTML")
        var spaced = Regex.Replace(name, @"(?<=[a-z])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])", " ");
        return char.ToUpperInvariant(spaced[0]) + spaced[1..];
    }

    #endregion
}
