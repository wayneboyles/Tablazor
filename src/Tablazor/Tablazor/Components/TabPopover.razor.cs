using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// A popover component that displays contextual content anchored to a trigger element.
/// Supports click, hover, and focus triggers with top, bottom, start, and end placements.
/// </summary>
/// <remarks>
/// Wrap the trigger element in <see cref="TriggerContent"/> (or use <see cref="ChildContent"/> for
/// simple cases) and provide the popover body via <see cref="Content"/> or <see cref="PopoverContent"/>.
/// </remarks>
/// <example>
/// Simple usage:
/// <code>
/// &lt;TabPopover Title="Info" Content="Helpful details here."&gt;
///     &lt;button class="btn btn-primary"&gt;Click me&lt;/button&gt;
/// &lt;/TabPopover&gt;
/// </code>
/// Rich content with named slots:
/// <code>
/// &lt;TabPopover Title="Details" Placement="PopoverPlacement.End" Trigger="PopoverTrigger.Hover"&gt;
///     &lt;TriggerContent&gt;
///         &lt;TabBadge Color="TabColors.Info"&gt;Hover me&lt;/TabBadge&gt;
///     &lt;/TriggerContent&gt;
///     &lt;PopoverContent&gt;
///         &lt;strong&gt;Rich&lt;/strong&gt; popover content.
///     &lt;/PopoverContent&gt;
/// &lt;/TabPopover&gt;
/// </code>
/// </example>
public partial class TabPopover : TabBaseComponent
{
    private bool _isVisible;
    private bool _prevIsOpen;
    private string _popoverId = string.Empty;
    private IJSObjectReference? _jsModule;
    private DotNetObjectReference<TabPopover>? _dotNetRef;

    /// <summary>
    /// Gets or sets the trigger element content. When set, this takes precedence over <see cref="ChildContent"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? TriggerContent { get; set; }

    /// <summary>
    /// Gets or sets the default child content, used as the trigger element when <see cref="TriggerContent"/> is not set.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the rich content to display inside the popover body.
    /// Takes precedence over <see cref="Content"/> when set.
    /// </summary>
    [Parameter]
    public RenderFragment? PopoverContent { get; set; }

    /// <summary>
    /// Gets or sets the plain-text content displayed in the popover body.
    /// Ignored when <see cref="PopoverContent"/> is set.
    /// </summary>
    [Parameter]
    public string? Content { get; set; }

    /// <summary>
    /// Gets or sets the title displayed in the popover header.
    /// When empty, the header is omitted.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the placement of the popover relative to the trigger element.
    /// </summary>
    /// <value>The default value is <see cref="PopoverPlacement.Top"/>.</value>
    [Parameter]
    public PopoverPlacement Placement { get; set; } = PopoverPlacement.Top;

    /// <summary>
    /// Gets or sets how the popover is triggered.
    /// </summary>
    /// <value>The default value is <see cref="PopoverTrigger.Click"/>.</value>
    [Parameter]
    public PopoverTrigger Trigger { get; set; } = PopoverTrigger.Click;

    /// <summary>
    /// Gets or sets a value indicating whether the popover is currently visible.
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
    /// Gets or sets the callback invoked when the popover is shown.
    /// </summary>
    [Parameter]
    public EventCallback OnShown { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the popover is hidden.
    /// </summary>
    [Parameter]
    public EventCallback OnHidden { get; set; }

    /// <summary>
    /// Gets a value indicating whether the popover is currently visible.
    /// </summary>
    public bool IsPopoverVisible => _isVisible;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _isVisible = IsOpen;
        _prevIsOpen = IsOpen;
        _popoverId = GetId();
        _dotNetRef = DotNetObjectReference.Create(this);
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (IsOpen != _prevIsOpen)
        {
            _prevIsOpen = IsOpen;
            _isVisible = IsOpen;
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender && JsRuntime is not null)
        {
            _jsModule = await JsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Tablazor/Tablazor/Components/TabPopover.razor.js");
        }
    }

    /// <summary>
    /// Shows the popover.
    /// </summary>
    public async Task ShowAsync()
    {
        if (_isVisible)
        {
            return;
        }

        _isVisible = true;

        if (Trigger == PopoverTrigger.Click)
        {
            await RegisterClickOutsideAsync();
        }

        if (IsOpenChanged.HasDelegate)
        {
            await IsOpenChanged.InvokeAsync(true);
        }

        if (OnShown.HasDelegate)
        {
            await OnShown.InvokeAsync();
        }

        StateHasChanged();
    }

    /// <summary>
    /// Hides the popover.
    /// </summary>
    public async Task HideAsync()
    {
        await HideInternalAsync(notifyJs: true);
    }

    /// <summary>
    /// Toggles the popover visibility.
    /// </summary>
    public async Task ToggleAsync()
    {
        if (_isVisible)
        {
            await HideAsync();
        }
        else
        {
            await ShowAsync();
        }
    }

    /// <summary>
    /// Called by JavaScript when a click outside the popover wrapper is detected.
    /// </summary>
    [JSInvokable]
    public async Task CloseFromJs()
    {
        await HideInternalAsync(notifyJs: false);
    }

    private async Task HideInternalAsync(bool notifyJs)
    {
        if (!_isVisible)
        {
            return;
        }

        _isVisible = false;

        if (notifyJs && Trigger == PopoverTrigger.Click)
        {
            await UnregisterClickOutsideAsync();
        }

        if (IsOpenChanged.HasDelegate)
        {
            await IsOpenChanged.InvokeAsync(false);
        }

        if (OnHidden.HasDelegate)
        {
            await OnHidden.InvokeAsync();
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task HandleClickAsync()
    {
        if (Trigger == PopoverTrigger.Click)
        {
            await ToggleAsync();
        }
    }

    private async Task HandleMouseEnterAsync()
    {
        if (Trigger == PopoverTrigger.Hover)
        {
            await ShowAsync();
        }
    }

    private async Task HandleMouseLeaveAsync()
    {
        if (Trigger == PopoverTrigger.Hover)
        {
            await HideAsync();
        }
    }

    private async Task HandleFocusInAsync()
    {
        if (Trigger == PopoverTrigger.Focus)
        {
            await ShowAsync();
        }
    }

    private async Task HandleFocusOutAsync()
    {
        if (Trigger == PopoverTrigger.Focus)
        {
            await HideAsync();
        }
    }

    private async Task RegisterClickOutsideAsync()
    {
        if (_jsModule is null || !IsJsRuntimeAvailable || _dotNetRef is null)
        {
            return;
        }

        try
        {
            await _jsModule.InvokeVoidAsync("registerClickOutside", _dotNetRef, _popoverId, Element);
        }
        catch (JSDisconnectedException) { }
    }

    private async Task UnregisterClickOutsideAsync()
    {
        if (_jsModule is null || !IsJsRuntimeAvailable)
        {
            return;
        }

        try
        {
            await _jsModule.InvokeVoidAsync("unregister", _popoverId);
        }
        catch (JSDisconnectedException) { }
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("position-relative d-inline-block")
            .AddClass(CssClass)
            .Build();
    }

    private string GetPopoverCssClass()
    {
        return new CssBuilder("popover")
            .AddClass($"bs-popover-{Placement.GetCssClassName()}")
            .AddClass("show")
            .Build();
    }

    private string GetPopoverPositionStyle()
    {
        // position:absolute must be explicit so it wins over any cascade ordering.
        // width:max-content + max-width:276px gives natural sizing without collapsing to the
        // trigger's width (the inline-block containing block), while still allowing text to wrap.
        const string common = "position: absolute; width: max-content; max-width: 276px; white-space: normal; word-wrap: break-word; z-index: 1070;";
        return Placement switch
        {
            PopoverPlacement.Top    => $"{common} bottom: calc(100% + 0.5rem); top: auto; left: 50%; right: auto; transform: translateX(-50%);",
            PopoverPlacement.Bottom => $"{common} top: calc(100% + 0.5rem); bottom: auto; left: 50%; right: auto; transform: translateX(-50%);",
            PopoverPlacement.Start  => $"{common} right: calc(100% + 0.5rem); left: auto; top: 50%; bottom: auto; transform: translateY(-50%);",
            PopoverPlacement.End    => $"{common} left: calc(100% + 0.5rem); right: auto; top: 50%; bottom: auto; transform: translateY(-50%);",
            _                       => $"{common} bottom: calc(100% + 0.5rem); top: auto; left: 50%; right: auto; transform: translateX(-50%);"
        };
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        if (_jsModule is not null)
        {
            try
            {
                await _jsModule.InvokeVoidAsync("unregister", _popoverId);
                await _jsModule.DisposeAsync();
            }
            catch (JSDisconnectedException) { }
        }

        _dotNetRef?.Dispose();

        await base.DisposeAsync();
    }
}
