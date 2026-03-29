using Microsoft.AspNetCore.Components;

using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// A data-driven table component that supports sortable columns, row selection,
/// row actions, custom cell templates, and an optional card wrapper.
/// </summary>
/// <typeparam name="TItem">The type of data item displayed in each table row.</typeparam>
public partial class TabTable<TItem> : TabBaseComponent
{
    private readonly List<TabTableColumn<TItem>> _columns = [];
    private readonly HashSet<TItem> _selectedItems = [];
    private TabTableColumn<TItem>? _sortColumn;
    private bool _sortAscending = true;
    private IEnumerable<TItem> _displayItems = [];

    /// <summary>
    /// Gets or sets the collection of data items to display.
    /// </summary>
    [Parameter]
    public IEnumerable<TItem> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the column definition child content.
    /// Add <see cref="TabTableColumn{TItem}"/> components inside the table to define columns.
    /// </summary>
    [Parameter]
    public RenderFragment? ColumnDefinitions { get; set; }

    /// <summary>
    /// Gets or sets a template rendered in the last cell of each row for row-level actions
    /// such as edit or delete buttons.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? RowActions { get; set; }

    /// <summary>
    /// Gets or sets content rendered in the card header. Overrides <see cref="Title"/> when set.
    /// Only used when <see cref="WrapInCard"/> is <c>true</c>.
    /// </summary>
    [Parameter]
    public RenderFragment? HeaderContent { get; set; }

    /// <summary>
    /// Gets or sets content shown when <see cref="Items"/> is empty.
    /// Overrides <see cref="EmptyText"/> when set.
    /// </summary>
    [Parameter]
    public RenderFragment? EmptyContent { get; set; }

    /// <summary>
    /// Gets or sets the title displayed in the card header.
    /// Only used when <see cref="WrapInCard"/> is <c>true</c> and <see cref="HeaderContent"/> is not set.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the message displayed when <see cref="Items"/> is empty.
    /// Defaults to "No data available."
    /// </summary>
    [Parameter]
    public string EmptyText { get; set; } = "No data available.";

    /// <summary>
    /// Gets or sets a value indicating whether to wrap the table in a Tabler card.
    /// </summary>
    /// <value>Defaults to <c>true</c>.</value>
    [Parameter]
    public bool WrapInCard { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to wrap the table in a responsive container.
    /// </summary>
    /// <value>Defaults to <c>true</c>.</value>
    [Parameter]
    public bool Responsive { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to apply alternating row shading.
    /// </summary>
    [Parameter]
    public bool Striped { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether rows are highlighted on hover.
    /// </summary>
    [Parameter]
    public bool Hover { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to use compact row padding.
    /// </summary>
    [Parameter]
    public bool Small { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether rows can be selected via checkboxes.
    /// </summary>
    [Parameter]
    public bool Selectable { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the selection changes.
    /// Receives the current set of selected items.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<TItem>> OnSelectionChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when a row is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<TItem> OnRowClick { get; set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        RefreshDisplayItems();
    }

    /// <summary>
    /// Registers a column with this table. Called by child <see cref="TabTableColumn{TItem}"/> components.
    /// </summary>
    internal void AddColumn(TabTableColumn<TItem> column)
    {
        _columns.Add(column);
        StateHasChanged();
    }

    /// <summary>
    /// Unregisters a column from this table.
    /// </summary>
    internal void RemoveColumn(TabTableColumn<TItem> column)
    {
        _columns.Remove(column);
        StateHasChanged();
    }

    /// <summary>
    /// Gets the currently selected items.
    /// </summary>
    public IReadOnlyCollection<TItem> SelectedItems => _selectedItems;

    /// <summary>
    /// Clears the current selection.
    /// </summary>
    public async Task ClearSelectionAsync()
    {
        _selectedItems.Clear();
        StateHasChanged();

        if (OnSelectionChanged.HasDelegate)
        {
            await OnSelectionChanged.InvokeAsync(_selectedItems);
        }
    }

    private void RefreshDisplayItems()
    {
        if (_sortColumn?.Field is null)
        {
            _displayItems = Items ?? [];
            return;
        }

        var sorted = _sortAscending
            ? Items.OrderBy(i => _sortColumn.Field(i))
            : Items.OrderByDescending(i => _sortColumn.Field(i));

        _displayItems = sorted;
    }

    private void SortBy(TabTableColumn<TItem> column)
    {
        if (_sortColumn == column)
        {
            _sortAscending = !_sortAscending;
        }
        else
        {
            _sortColumn = column;
            _sortAscending = true;
        }

        RefreshDisplayItems();
    }

    private bool IsAllSelected()
        => _displayItems.Any() && _displayItems.All(i => _selectedItems.Contains(i));

    private async Task ToggleSelectAll(ChangeEventArgs e)
    {
        var isChecked = e.Value is true;

        if (isChecked)
        {
            foreach (var item in _displayItems)
            {
                _selectedItems.Add(item);
            }
        }
        else
        {
            _selectedItems.Clear();
        }

        if (OnSelectionChanged.HasDelegate)
        {
            await OnSelectionChanged.InvokeAsync(_selectedItems);
        }
    }

    private async Task ToggleSelectItem(TItem item)
    {
        if (!_selectedItems.Remove(item))
        {
            _selectedItems.Add(item);
        }

        if (OnSelectionChanged.HasDelegate)
        {
            await OnSelectionChanged.InvokeAsync(_selectedItems);
        }
    }

    private async Task HandleRowClick(TItem item)
    {
        if (OnRowClick.HasDelegate)
        {
            await OnRowClick.InvokeAsync(item);
        }
    }

    private int GetColspan()
    {
        var count = _columns.Count;
        if (Selectable) count++;
        if (RowActions is not null) count++;
        return count;
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("table table-vcenter")
            .AddClass("card-table", WrapInCard)
            .AddClass("table-striped", Striped)
            .AddClass("table-hover", Hover)
            .AddClass("table-sm", Small)
            .AddClass(CssClass)
            .Build();
    }
}
