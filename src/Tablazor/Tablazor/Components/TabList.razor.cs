using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// A generic list component that renders a collection of items as a Tabler list-group.
/// Supports avatars, icons, text slots, row selection, and bulk actions.
/// </summary>
/// <typeparam name="TItem">The type of data item displayed in each list row.</typeparam>
public partial class TabList<TItem> : TabBaseComponent
{
    private readonly HashSet<TItem> _selectedItems = [];
    private List<TItem> _items = [];

    /// <summary>
    /// Gets or sets the collection of data items to display.
    /// </summary>
    [Parameter]
    public IEnumerable<TItem> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the title displayed in the card header.
    /// Only used when <see cref="WrapInCard"/> is <c>true</c> and <see cref="HeaderContent"/> is not set.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets whether to wrap the list in a Tabler card.
    /// </summary>
    /// <value>Defaults to <c>true</c>.</value>
    [Parameter]
    public bool WrapInCard { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to apply flush styling, removing outer borders.
    /// Recommended when <see cref="WrapInCard"/> is <c>true</c>.
    /// </summary>
    /// <value>Defaults to <c>true</c>.</value>
    [Parameter]
    public bool Flush { get; set; } = true;

    /// <summary>
    /// Gets or sets whether items can be selected via checkboxes.
    /// </summary>
    [Parameter]
    public bool Selectable { get; set; }

    /// <summary>
    /// Gets or sets content shown in the header when one or more rows are selected.
    /// Receives the current set of selected items.
    /// </summary>
    [Parameter]
    public RenderFragment<IEnumerable<TItem>>? BulkActions { get; set; }

    /// <summary>
    /// Gets or sets custom content rendered in the card header.
    /// Overrides <see cref="Title"/> and the select-all control when set.
    /// </summary>
    [Parameter]
    public RenderFragment? HeaderContent { get; set; }

    /// <summary>
    /// Gets or sets the template for the avatar or icon column (left side of each row).
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? AvatarContent { get; set; }

    /// <summary>
    /// Gets or sets the template for the main title text of each item.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? TitleContent { get; set; }

    /// <summary>
    /// Gets or sets the template for the subtitle or secondary text of each item.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? SubtitleContent { get; set; }

    /// <summary>
    /// Gets or sets the template for the right-side content of each item (badges, actions, metadata).
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? RightContent { get; set; }

    /// <summary>
    /// Gets or sets the message displayed when <see cref="Items"/> is empty.
    /// </summary>
    /// <value>Defaults to "No items."</value>
    [Parameter]
    public string EmptyText { get; set; } = "No items.";

    /// <summary>
    /// Gets or sets custom content rendered when <see cref="Items"/> is empty.
    /// Overrides <see cref="EmptyText"/> when set.
    /// </summary>
    [Parameter]
    public RenderFragment? EmptyContent { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when an item row is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<TItem> OnItemClick { get; set; }

    /// <summary>
    /// Gets the set of currently selected items.
    /// </summary>
    public IReadOnlyCollection<TItem> SelectedItems => _selectedItems;

    private bool AllSelected => _items.Count > 0 && _items.All(i => _selectedItems.Contains(i));

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        _items = Items.ToList();
        base.OnParametersSet();
        _selectedItems.IntersectWith(_items);
    }

    private void ToggleSelectAll()
    {
        if (AllSelected)
        {
            _selectedItems.Clear();
        }
        else
        {
            foreach (var item in _items)
            {
                _selectedItems.Add(item);
            }
        }
    }

    private void ToggleItem(TItem item)
    {
        if (!_selectedItems.Remove(item))
        {
            _selectedItems.Add(item);
        }
    }

    private bool IsSelected(TItem item) => _selectedItems.Contains(item);

    private async Task HandleItemClickAsync(TItem item)
    {
        if (OnItemClick.HasDelegate)
        {
            await OnItemClick.InvokeAsync(item);
        }
    }

    private string GetItemCssClass(TItem item)
    {
        return new CssBuilder("list-group-item")
            .AddClass("active", IsSelected(item))
            .AddClass("list-group-item-action", OnItemClick.HasDelegate)
            .Build();
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("list-group")
            .AddClass("list-group-flush", Flush)
            .AddClass(CssClass)
            .Build();
    }
}
