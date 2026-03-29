using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// An accordion component that manages collapsible content panels.
/// Add <see cref="TabAccordionItem"/> components as child content to define panels.
/// </summary>
public partial class TabAccordion : TabBaseComponent
{
    private readonly List<TabAccordionItem> _items = [];
    private readonly HashSet<TabAccordionItem> _openItems = [];

    /// <summary>
    /// Gets or sets the accordion panel content. Add <see cref="TabAccordionItem"/> components here.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether multiple panels can be open simultaneously.
    /// When <c>false</c> (default), opening one panel closes all others.
    /// </summary>
    [Parameter]
    public bool AlwaysOpen { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to apply flush styling, removing outer borders and
    /// rounded corners so the accordion renders edge-to-edge within its parent container.
    /// </summary>
    [Parameter]
    public bool Flush { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when any accordion item is opened.
    /// Receives the <see cref="TabAccordionItem"/> that was opened.
    /// </summary>
    [Parameter]
    public EventCallback<TabAccordionItem> OnItemOpened { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when any accordion item is closed.
    /// Receives the <see cref="TabAccordionItem"/> that was closed.
    /// </summary>
    [Parameter]
    public EventCallback<TabAccordionItem> OnItemClosed { get; set; }

    /// <summary>
    /// Gets the set of currently open accordion items.
    /// </summary>
    public IReadOnlyCollection<TabAccordionItem> OpenItems => _openItems;

    /// <summary>
    /// Registers an item with this accordion. Called automatically by child <see cref="TabAccordionItem"/> components.
    /// </summary>
    internal void RegisterItem(TabAccordionItem item)
    {
        _items.Add(item);
        if (item.IsOpen)
        {
            _openItems.Add(item);
        }
    }

    /// <summary>
    /// Unregisters an item from this accordion.
    /// </summary>
    internal void UnregisterItem(TabAccordionItem item)
    {
        _items.Remove(item);
        _openItems.Remove(item);
    }

    /// <summary>
    /// Toggles the specified item open or closed.
    /// When <see cref="AlwaysOpen"/> is <c>false</c>, also closes all other open items.
    /// </summary>
    internal async Task ToggleItemAsync(TabAccordionItem item)
    {
        var wasOpen = _openItems.Contains(item);

        if (!AlwaysOpen)
        {
            var toClose = _openItems.Where(i => i != item).ToList();
            foreach (var other in toClose)
            {
                _openItems.Remove(other);
                if (OnItemClosed.HasDelegate)
                {
                    await OnItemClosed.InvokeAsync(other);
                }
            }
        }

        if (wasOpen)
        {
            _openItems.Remove(item);
            if (OnItemClosed.HasDelegate)
            {
                await OnItemClosed.InvokeAsync(item);
            }
        }
        else
        {
            _openItems.Add(item);
            if (OnItemOpened.HasDelegate)
            {
                await OnItemOpened.InvokeAsync(item);
            }
        }

        StateHasChanged();
    }

    /// <summary>
    /// Returns whether the specified item is currently open.
    /// </summary>
    internal bool IsItemOpen(TabAccordionItem item) => _openItems.Contains(item);

    /// <summary>
    /// Opens the specified item programmatically. Has no effect if already open.
    /// </summary>
    public async Task OpenItemAsync(TabAccordionItem item)
    {
        if (!_openItems.Contains(item))
        {
            await ToggleItemAsync(item);
        }
    }

    /// <summary>
    /// Closes the specified item programmatically. Has no effect if already closed.
    /// </summary>
    public async Task CloseItemAsync(TabAccordionItem item)
    {
        if (_openItems.Contains(item))
        {
            await ToggleItemAsync(item);
        }
    }

    /// <summary>
    /// Opens all registered items. Primarily useful when <see cref="AlwaysOpen"/> is <c>true</c>.
    /// </summary>
    public async Task OpenAllAsync()
    {
        foreach (var item in _items.Where(i => !_openItems.Contains(i)).ToList())
        {
            _openItems.Add(item);
            if (OnItemOpened.HasDelegate)
            {
                await OnItemOpened.InvokeAsync(item);
            }
        }

        StateHasChanged();
    }

    /// <summary>
    /// Closes all open items.
    /// </summary>
    public async Task CloseAllAsync()
    {
        foreach (var item in _openItems.ToList())
        {
            _openItems.Remove(item);
            if (OnItemClosed.HasDelegate)
            {
                await OnItemClosed.InvokeAsync(item);
            }
        }

        StateHasChanged();
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("accordion")
            .AddClass("accordion-flush", Flush)
            .AddClass(CssClass)
            .Build();
    }
}
