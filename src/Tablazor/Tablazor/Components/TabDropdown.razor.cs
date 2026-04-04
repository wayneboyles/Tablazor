using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// A dropdown component that displays a toggleable menu of items.
/// Based on the Tabler design system.
/// </summary>
/// <remarks>
/// Use <see cref="TabDropdownMenu"/> as a child to define the menu container,
/// and <see cref="TabDropdownItem"/>, <see cref="TabDropdownDivider"/>, and
/// <see cref="TabDropdownHeader"/> for menu contents. The toggle element can be
/// any child content such as a <see cref="TabButton"/>.
/// </remarks>
/// <example>
/// <code>
/// &lt;TabDropdown&gt;
///     &lt;TabButton Color="TabColors.Primary" OnClick="@(() =&gt; dropdown.ToggleAsync())"&gt;
///         Open Menu
///     &lt;/TabButton&gt;
///     &lt;TabDropdownMenu&gt;
///         &lt;TabDropdownItem&gt;Action&lt;/TabDropdownItem&gt;
///         &lt;TabDropdownDivider /&gt;
///         &lt;TabDropdownItem&gt;Another Action&lt;/TabDropdownItem&gt;
///     &lt;/TabDropdownMenu&gt;
/// &lt;/TabDropdown&gt;
/// </code>
/// </example>
public partial class TabDropdown : TabBaseComponent
{
    private bool _isOpen;
    private bool _prevIsOpen;
    private string _dropdownId = string.Empty;
    private IJSObjectReference? _jsModule;
    private DotNetObjectReference<TabDropdown>? _dotNetRef;
    private readonly List<TabDropdownMenu> _menus = [];

    /// <summary>
    /// Gets or sets the child content of the dropdown, which should include
    /// a toggle element and a <see cref="TabDropdownMenu"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the direction in which the dropdown menu opens.
    /// </summary>
    /// <value>The default value is <see cref="DropdownDirection.Down"/>.</value>
    [Parameter]
    public DropdownDirection Direction { get; set; } = DropdownDirection.Down;

    /// <summary>
    /// Gets or sets a value indicating whether the dropdown is currently open.
    /// </summary>
    [Parameter]
    public bool IsOpen { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the dropdown open state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the dropdown is opened.
    /// </summary>
    [Parameter]
    public EventCallback OnOpened { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the dropdown is closed.
    /// </summary>
    [Parameter]
    public EventCallback OnClosed { get; set; }

    /// <summary>
    /// Gets a value indicating whether the dropdown menu is currently visible.
    /// </summary>
    public bool IsMenuOpen => _isOpen;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _isOpen = IsOpen;
        _prevIsOpen = IsOpen;
        _dropdownId = GetId();
        _dotNetRef = DotNetObjectReference.Create(this);
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (IsOpen != _prevIsOpen)
        {
            _prevIsOpen = IsOpen;
            _isOpen = IsOpen;
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender && JsRuntime is not null)
        {
            _jsModule = await JsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Tablazor/Tablazor/Components/TabDropdown.razor.js");
        }
    }

    /// <summary>
    /// Toggles the dropdown menu open or closed.
    /// </summary>
    public async Task ToggleAsync()
    {
        if (_isOpen)
        {
            await CloseAsync();
        }
        else
        {
            await OpenAsync();
        }
    }

    /// <summary>
    /// Opens the dropdown menu.
    /// </summary>
    public async Task OpenAsync()
    {
        if (_isOpen)
        {
            return;
        }

        _isOpen = true;

        await OpenJsAsync();

        if (IsOpenChanged.HasDelegate)
        {
            await IsOpenChanged.InvokeAsync(_isOpen);
        }

        if (OnOpened.HasDelegate)
        {
            await OnOpened.InvokeAsync();
        }

        NotifyMenus();
        StateHasChanged();
    }

    /// <summary>
    /// Closes the dropdown menu.
    /// </summary>
    public async Task CloseAsync()
    {
        await CloseInternalAsync(notifyJs: true);
    }

    /// <summary>
    /// Called by JavaScript when a click outside the dropdown is detected,
    /// or when another dropdown opens.
    /// </summary>
    [JSInvokable]
    public async Task CloseFromJs()
    {
        await CloseInternalAsync(notifyJs: false);
    }

    /// <summary>
    /// Registers a <see cref="TabDropdownMenu"/> child component for state change notifications.
    /// </summary>
    /// <param name="menu">The menu component to register.</param>
    internal void RegisterMenu(TabDropdownMenu menu)
    {
        if (!_menus.Contains(menu))
        {
            _menus.Add(menu);
        }
    }

    /// <summary>
    /// Unregisters a <see cref="TabDropdownMenu"/> child component.
    /// </summary>
    /// <param name="menu">The menu component to unregister.</param>
    internal void UnregisterMenu(TabDropdownMenu menu)
    {
        _menus.Remove(menu);
    }

    private async Task CloseInternalAsync(bool notifyJs)
    {
        if (!_isOpen)
        {
            return;
        }

        _isOpen = false;

        if (notifyJs)
        {
            await CloseJsAsync();
        }

        if (IsOpenChanged.HasDelegate)
        {
            await IsOpenChanged.InvokeAsync(_isOpen);
        }

        if (OnClosed.HasDelegate)
        {
            await OnClosed.InvokeAsync();
        }

        NotifyMenus();
        StateHasChanged();
    }

    private void NotifyMenus()
    {
        foreach (var menu in _menus)
        {
            menu.NotifyStateChanged();
        }
    }

    private async Task OpenJsAsync()
    {
        if (_jsModule is null || !IsJsRuntimeAvailable || _dotNetRef is null)
        {
            return;
        }

        try
        {
            await _jsModule.InvokeVoidAsync("open", _dotNetRef, _dropdownId, Element);
        }
        catch (JSDisconnectedException) { }
    }

    private async Task CloseJsAsync()
    {
        if (_jsModule is null || !IsJsRuntimeAvailable)
        {
            return;
        }

        try
        {
            await _jsModule.InvokeVoidAsync("close", _dropdownId);
        }
        catch (JSDisconnectedException) { }
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder(Direction.GetCssClassName())
            .AddClass(CssClass)
            .Build();
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        if (_jsModule is not null)
        {
            try
            {
                await _jsModule.InvokeVoidAsync("dispose", _dropdownId);
                await _jsModule.DisposeAsync();
            }
            catch (JSDisconnectedException) { }
        }

        _dotNetRef?.Dispose();

        await base.DisposeAsync();
    }
}
