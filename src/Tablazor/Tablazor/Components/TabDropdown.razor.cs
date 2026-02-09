using Microsoft.AspNetCore.Components;
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
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (IsOpen != _isOpen)
        {
            _isOpen = IsOpen;
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
        if (!_isOpen)
        {
            return;
        }

        _isOpen = false;

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

    private void NotifyMenus()
    {
        foreach (var menu in _menus)
        {
            menu.NotifyStateChanged();
        }
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder(Direction.GetCssClassName())
            .AddClass(CssClass)
            .Build();
    }
}
