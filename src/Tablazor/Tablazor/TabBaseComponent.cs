using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Tablazor.Exceptions;
using Tablazor.Validation;

namespace Tablazor;

/// <summary>
/// Base component that all Tablazor components inherit from
/// </summary>
public abstract class TabBaseComponent : ComponentBase, IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets or sets the logger factory for this component to build
    /// an <see cref="ILogger"/>
    /// </summary>
    [Inject] 
    private ILoggerFactory LoggerFactory { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the js runtime.
    /// </summary>
    /// <value>The js runtime.</value>
    [Inject]
    protected IJSRuntime? JsRuntime { get; set; }
    
#if NET10_0_OR_GREATER
    
    /// <summary>
    /// Gets a reference to the logger for the component.
    /// </summary>
    protected ILogger Logger => field ??= LoggerFactory.CreateLogger(GetType());
    
#else

    private ILogger? _logger;

    /// <summary>
    /// Gets a reference to the logger for the component.
    /// </summary>
    protected ILogger Logger => _logger ??= LoggerFactory.CreateLogger(GetType());

#endif
    /// <summary>
    /// Gets a reference to the HTML element rendered by this component.
    /// Can be used with JavaScript interop or for programmatic DOM manipulation.
    /// The reference is only valid after the component has been rendered (after OnAfterRender).
    /// </summary>
    /// <value>The element reference to the rendered HTML element.</value>
    protected ElementReference Element { get; set; }
    
    /// <summary>
    /// Gets or sets one or more CSS class names to apply to the component.
    /// </summary>
    [Parameter]
    public string? CssClass { get; set; }

    /// <summary>
    /// Gets or sets the CSS styles to apply to the rendered element.
    /// </summary>
    /// <remarks>Use this property to specify inline CSS styles for the component. The value should be
    /// a valid CSS style string, such as "color: red; font-size: 14px;". This property is typically used to
    /// customize the appearance of the component at runtime.</remarks>
    [Parameter]
    public string? Style { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the component is visible.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Whether the component is disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }
    
    /// <summary>
    /// Will capture all unmatched attributes and add them to the root element of the component.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    /// <summary>
    /// Gets or sets a callback that is invoked when validation fails.
    /// If not set, validation errors will throw an <see cref="TabException"/>.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<string>> OnValidationFailed { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether <see cref="JsRuntime" /> is available.
    /// </summary>
    protected bool IsJsRuntimeAvailable { get; set; }
    
    /// <summary>
    /// Generates a unique ID for the component instance if an ID is not already assigned.
    /// </summary>
    /// <returns></returns>
    protected string GetId()
    {
        object? id = null;

        var haveId = AdditionalAttributes?.TryGetValue("id", out id);
        
        if (haveId != true)
        {
            return IdBuilder.NextCompact();
        }

        if (id is string idString && !string.IsNullOrWhiteSpace(idString))
        {
            return idString;
        }

        return IdBuilder.NextCompact();
    }
    
    /// <summary>
    /// Gets the CSS class provided by the user and combines it with the
    /// component CSS classes
    /// </summary>
    /// <returns></returns>
    protected string GetCssClass()
    {
        if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("class", out var @class) && !string.IsNullOrEmpty(Convert.ToString(@class)))
        {
            return $"{BuildCssClass()} {@class}";
        }

        return BuildCssClass();
    }

    /// <summary>
    /// Gets the CSS Style attributes provided by the user and combines it with
    /// the component Style attributes
    /// </summary>
    /// <returns></returns>
    protected string GetStyleString()
    {
        if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("style", out var @style) && !string.IsNullOrEmpty(Convert.ToString(@style)))
        {
            return $"{BuildStyleString()} {@style}";
        }
        
        return BuildStyleString();
    }

    /// <summary>
    /// Method invoked when the component has received parameters from its parent in
    /// the render tree, and the incoming values have been assigned to properties.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        ValidateComponent();
    }
    
    /// <summary>
    /// Validates the current component instance and throws an exception if validation fails.
    /// </summary>
    /// <remarks>Override this method in a derived class to customize the validation logic for the
    /// component. This method is typically called before performing operations that require the component to be in
    /// a valid state.</remarks>
    /// <exception cref="InvalidOperationException">Thrown if the component fails validation. The exception message contains details about the validation
    /// errors.</exception>
    protected virtual void ValidateComponent()
    {
        var errors = ComponentValidator.Validate(this).ToList();

        if (errors.Count == 0)
        {
            return;
        }

        if (OnValidationFailed.HasDelegate)
        {
            OnValidationFailed.InvokeAsync(errors);
        }
        else
        {
            var componentName = GetType().Name;
                
            var errorMessage =
                $"Parameter validation failed for component '{componentName}':{Environment.NewLine}" +
                string.Join(Environment.NewLine, errors.Select(e => $"  - {e}"));

            throw new TabException(errorMessage);
        }
    }

    /// <summary>
    /// Method invoked after each time the component has rendered interactively and the UI has finished
    /// updating (for example, after elements have been added to the browser DOM). Any <see cref="T:Microsoft.AspNetCore.Components.ElementReference" />
    /// fields will be populated by the time this runs.
    /// This method is not invoked during prerendering or server-side rendering, because those processes
    /// are not attached to any live browser DOM and are already complete before the DOM is updated.
    /// </summary>
    /// <param name="firstRender">
    /// Set to <c>true</c> if this is the first time <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRender(System.Boolean)" /> has been invoked
    /// on this component instance; otherwise <c>false</c>.
    /// </param>
    /// <remarks>
    /// The <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRender(System.Boolean)" /> and <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRenderAsync(System.Boolean)" /> lifecycle methods
    /// are useful for performing interop, or interacting with values received from <c>@ref</c>.
    /// Use the <paramref name="firstRender" /> parameter to ensure that initialization work is only performed
    /// once.
    /// </remarks>
    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (JsRuntime is not null)
        {
            IsJsRuntimeAvailable = true;
        }
    }

    /// <summary>
    /// Builds a CSS class attribute value by combining the specified base class with any additional CSS classes
    /// defined in the current instance.
    /// </summary>
    /// <returns>A string containing the combined CSS classes, separated by spaces. If no additional CSS classes are defined,
    /// returns the base class.</returns>
    protected virtual string BuildCssClass()
    {
        return string.Empty;
    }

    /// <summary>
    /// Builds a CSS style string by combinding the specified base class with any additional
    /// Style attributes defined in the current instance
    /// </summary>
    /// <returns>A string containing the combined Style</returns>
    protected virtual string BuildStyleString()
    {
        return string.Empty;
    }
    
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public virtual void Dispose()
    {
        LoggerFactory.Dispose();
        
        if (IsJsRuntimeAvailable)
        {
                
        }
        
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or
    /// resetting unmanaged resources asynchronously.</summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public virtual async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
    }
}