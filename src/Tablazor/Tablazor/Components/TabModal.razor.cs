using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// A modal dialog component based on the Tabler design system.
/// Supports various sizes, centered layout, scrollable content, and static backdrop options.
/// </summary>
/// <remarks>
/// Use <see cref="TabModalHeader"/>, <see cref="TabModalBody"/>, and <see cref="TabModalFooter"/>
/// as child components for structured modal layouts, or use the <see cref="Title"/> and <see cref="FooterContent"/>
/// parameters for simple scenarios.
/// </remarks>
public partial class TabModal : TabBaseComponent
{
    private string _modalId = string.Empty;
    private bool _isOpen;
    private bool _isShowing;
    private IJSObjectReference? _jsModule;

    /// <summary>
    /// Gets or sets the content to render inside the modal body.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the title to display in the modal header.
    /// If set, a default header with this title will be rendered.
    /// For custom headers, omit this parameter and include a <see cref="TabModalHeader"/> in the ChildContent.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the content to render in the modal footer.
    /// If set, a default footer will be rendered with this content.
    /// For custom footers, omit this parameter and include a <see cref="TabModalFooter"/> in the ChildContent.
    /// </summary>
    [Parameter]
    public RenderFragment? FooterContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show a close button in the modal header.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    [Parameter]
    public bool ShowCloseButton { get; set; } = true;

    /// <summary>
    /// Gets or sets the size of the modal.
    /// </summary>
    /// <value>The default value is <see cref="ModalSize.Default"/>.</value>
    [Parameter]
    public ModalSize Size { get; set; } = ModalSize.Default;

    /// <summary>
    /// Gets or sets a value indicating whether the modal should be vertically centered.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    [Parameter]
    public bool Centered { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the modal body should be scrollable.
    /// When true, the modal body will scroll if content exceeds the viewport height.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    [Parameter]
    public bool Scrollable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the backdrop is static.
    /// When true, clicking outside the modal will not close it.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    [Parameter]
    public bool StaticBackdrop { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the modal is currently open.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    [Parameter]
    public bool IsOpen { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the <see cref="IsOpen"/> value changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the modal is shown.
    /// </summary>
    [Parameter]
    public EventCallback OnShown { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the modal is hidden.
    /// </summary>
    [Parameter]
    public EventCallback OnHidden { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether animations should be enabled.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    [Parameter]
    public bool EnableAnimation { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to close the modal when the escape key is pressed.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    [Parameter]
    public bool CloseOnEscape { get; set; } = true;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _modalId = GetId();
        _isOpen = IsOpen;
        _isShowing = IsOpen;
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        // Handle changes to IsOpen parameter from parent
        if (_isOpen != IsOpen)
        {
            if (IsOpen)
            {
                _ = ShowAsync();
            }
            else
            {
                _ = HideAsync();
            }
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender && JsRuntime is not null)
        {
            _jsModule = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Tablazor/Tablazor/Components/TabModal.razor.js");
        }

        // Focus the modal when shown for keyboard accessibility
        // Only focus if the modal is visible (rendered) and showing
        if (Visible && _isOpen && _isShowing && IsJsRuntimeAvailable)
        {
            try
            {
                await Element.FocusAsync();
            }
            catch (JSException)
            {
                // Ignore focus errors
            }
            catch (InvalidOperationException)
            {
                // Ignore if ElementReference is not configured (e.g., in unit tests)
            }
        }
    }

    /// <summary>
    /// Shows the modal dialog.
    /// </summary>
    public async Task ShowAsync()
    {
        if (_isOpen)
        {
            return;
        }

        _isOpen = true;
        StateHasChanged();

        // Small delay to allow the DOM to update before adding the show class for animation
        if (EnableAnimation)
        {
            await Task.Delay(10);
        }

        _isShowing = true;
        await SetBodyScrollLockAsync(true);
        StateHasChanged();

        if (IsOpenChanged.HasDelegate)
        {
            await IsOpenChanged.InvokeAsync(true);
        }

        if (OnShown.HasDelegate)
        {
            await OnShown.InvokeAsync();
        }
    }

    /// <summary>
    /// Hides the modal dialog.
    /// </summary>
    public async Task HideAsync()
    {
        if (!_isOpen)
        {
            return;
        }

        _isShowing = false;
        StateHasChanged();

        // Wait for animation to complete before removing from DOM
        if (EnableAnimation)
        {
            await Task.Delay(150);
        }

        _isOpen = false;
        await SetBodyScrollLockAsync(false);
        StateHasChanged();

        if (IsOpenChanged.HasDelegate)
        {
            await IsOpenChanged.InvokeAsync(false);
        }

        if (OnHidden.HasDelegate)
        {
            await OnHidden.InvokeAsync();
        }
    }

    /// <summary>
    /// Toggles the modal visibility.
    /// </summary>
    public async Task ToggleAsync()
    {
        if (_isOpen)
        {
            await HideAsync();
        }
        else
        {
            await ShowAsync();
        }
    }

    /// <summary>
    /// Handles the close button click event.
    /// </summary>
    private async Task HandleCloseAsync()
    {
        await HideAsync();
    }

    /// <summary>
    /// Handles backdrop click event.
    /// </summary>
    private async Task HandleBackdropClickAsync()
    {
        if (!StaticBackdrop)
        {
            await HideAsync();
        }
    }

    /// <summary>
    /// Handles keyboard events for the modal.
    /// </summary>
    private async Task HandleKeyDownAsync(KeyboardEventArgs e)
    {
        if (CloseOnEscape && e.Key == "Escape")
        {
            await HideAsync();
        }
    }

    /// <summary>
    /// Builds the CSS class string for the modal wrapper element.
    /// </summary>
    /// <returns>A string containing the combined CSS classes.</returns>
    protected override string BuildCssClass()
    {
        return new CssBuilder("modal")
            .AddClass("fade", EnableAnimation)
            .AddClass("show", _isShowing)
            .AddClass(CssClass)
            .Build();
    }

    /// <summary>
    /// Builds the CSS class string for the modal dialog element.
    /// </summary>
    /// <returns>A string containing the combined CSS classes for the dialog.</returns>
    private string BuildDialogCssClass()
    {
        return new CssBuilder("modal-dialog")
            .AddClass($"modal-{Size.GetCssClassName()}", Size != ModalSize.Default)
            .AddClass("modal-dialog-centered", Centered)
            .AddClass("modal-dialog-scrollable", Scrollable)
            .Build();
    }

    /// <summary>
    /// Sets or removes the body scroll lock.
    /// </summary>
    private async Task SetBodyScrollLockAsync(bool locked)
    {
        if (_jsModule is not null && IsJsRuntimeAvailable)
        {
            try
            {
                await _jsModule.InvokeVoidAsync("setBodyScrollLock", locked);
            }
            catch (JSDisconnectedException)
            {
                // Ignore if JS runtime is disconnected
            }
        }
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        if (_jsModule is not null)
        {
            try
            {
                await _jsModule.InvokeVoidAsync("setBodyScrollLock", false);
                await _jsModule.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // Ignore if the JS runtime is disconnected
            }
        }

        await base.DisposeAsync();
    }
}
