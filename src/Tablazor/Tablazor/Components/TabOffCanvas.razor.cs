using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// An off-canvas panel that slides in from any edge of the viewport.
/// Supports titles, custom headers, body content, footer content, backdrop, and keyboard dismissal.
/// </summary>
/// <remarks>
/// Use <see cref="ShowAsync"/>, <see cref="HideAsync"/>, or <see cref="ToggleAsync"/> to control
/// visibility programmatically, or bind <see cref="IsOpen"/> for two-way state management.
/// </remarks>
public partial class TabOffCanvas : TabBaseComponent
{
    private bool _isOpen;
    private bool _isShowing;
    private string _offCanvasId = string.Empty;
    private IJSObjectReference? _jsModule;

    /// <summary>
    /// Gets or sets the main body content of the off-canvas panel.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets custom header content. When set, replaces the default
    /// <see cref="Title"/> / close-button header entirely.
    /// </summary>
    [Parameter]
    public RenderFragment? HeaderContent { get; set; }

    /// <summary>
    /// Gets or sets content rendered in a footer bar at the bottom of the panel,
    /// separated from the body by a border. Useful for action buttons.
    /// </summary>
    [Parameter]
    public RenderFragment? FooterContent { get; set; }

    /// <summary>
    /// Gets or sets the title displayed in the off-canvas header.
    /// Ignored when <see cref="HeaderContent"/> is provided.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets which edge of the viewport the panel slides in from.
    /// Defaults to <see cref="OffCanvasPlacement.End"/> (right).
    /// </summary>
    [Parameter]
    public OffCanvasPlacement Placement { get; set; } = OffCanvasPlacement.End;

    /// <summary>
    /// Gets or sets a value indicating whether to show a close button in the header.
    /// Ignored when <see cref="HeaderContent"/> is provided.
    /// </summary>
    /// <value>Defaults to <c>true</c>.</value>
    [Parameter]
    public bool ShowCloseButton { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether a backdrop overlay is rendered behind the panel.
    /// </summary>
    /// <value>Defaults to <c>true</c>.</value>
    [Parameter]
    public bool ShowBackdrop { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether CSS slide animations are enabled.
    /// </summary>
    /// <value>Defaults to <c>true</c>.</value>
    [Parameter]
    public bool EnableAnimation { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether clicking the backdrop closes the panel.
    /// When <c>true</c>, backdrop clicks are ignored.
    /// </summary>
    /// <value>Defaults to <c>false</c>.</value>
    [Parameter]
    public bool StaticBackdrop { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether pressing <c>Escape</c> closes the panel.
    /// </summary>
    /// <value>Defaults to <c>true</c>.</value>
    [Parameter]
    public bool CloseOnEscape { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the off-canvas panel is open.
    /// Supports two-way binding via <c>@bind-IsOpen</c>.
    /// </summary>
    [Parameter]
    public bool IsOpen { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when <see cref="IsOpen"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked after the panel has finished opening.
    /// </summary>
    [Parameter]
    public EventCallback OnShown { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked after the panel has finished closing.
    /// </summary>
    [Parameter]
    public EventCallback OnHidden { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _offCanvasId = GetId();
        _isOpen = IsOpen;
        _isShowing = IsOpen;
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (_isOpen != IsOpen)
        {
            _ = IsOpen ? ShowAsync() : HideAsync();
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender && JsRuntime is not null)
        {
            _jsModule = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Tablazor/Tablazor/Components/TabOffCanvas.razor.js");
        }

        if (Visible && _isOpen && _isShowing && IsJsRuntimeAvailable)
        {
            try
            {
                await Element.FocusAsync();
            }
            catch (JSException) { }
            catch (InvalidOperationException) { }
        }
    }

    /// <summary>
    /// Opens the off-canvas panel.
    /// </summary>
    public async Task ShowAsync()
    {
        if (_isOpen)
        {
            return;
        }

        _isOpen = true;
        StateHasChanged();

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
    /// Closes the off-canvas panel.
    /// </summary>
    public async Task HideAsync()
    {
        if (!_isOpen)
        {
            return;
        }

        _isShowing = false;
        StateHasChanged();

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
    /// Toggles the off-canvas panel open or closed.
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

    private async Task HandleCloseAsync() => await HideAsync();

    private async Task HandleBackdropClickAsync()
    {
        if (!StaticBackdrop)
        {
            await HideAsync();
        }
    }

    private async Task HandleKeyDownAsync(KeyboardEventArgs e)
    {
        if (CloseOnEscape && e.Key == "Escape")
        {
            await HideAsync();
        }
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("offcanvas")
            .AddClass($"offcanvas-{Placement.GetCssClassName()}")
            .AddClass("show", _isShowing)
            .AddClass(CssClass)
            .Build();
    }

    // ── JS interop ───────────────────────────────────────────────────────────

    private async Task SetBodyScrollLockAsync(bool locked)
    {
        if (_jsModule is null || !IsJsRuntimeAvailable) return;

        try
        {
            await _jsModule.InvokeVoidAsync("setBodyScrollLock", locked);
        }
        catch (JSDisconnectedException) { }
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
            catch (JSDisconnectedException) { }
        }

        await base.DisposeAsync();
    }
}
