using Microsoft.AspNetCore.Components;

using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// Defines a column in a <see cref="TabTable{TItem}"/>.
/// </summary>
/// <typeparam name="TItem">The type of data item displayed in the table.</typeparam>
public partial class TabTableColumn<TItem> : ComponentBase, IDisposable
{
    /// <summary>
    /// Gets or sets the parent <see cref="TabTable{TItem}"/> this column belongs to.
    /// </summary>
    [CascadingParameter]
    private TabTable<TItem>? ParentTable { get; set; }

    /// <summary>
    /// Gets or sets the column header text.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets a delegate that extracts the cell value from a data item.
    /// When set, the column renders the returned value as text.
    /// </summary>
    [Parameter]
    public Func<TItem, object?>? Field { get; set; }

    /// <summary>
    /// Gets or sets a custom cell template. When set, overrides <see cref="Field"/>
    /// and renders the template for each row's item.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? CellTemplate { get; set; }

    /// <summary>
    /// Gets or sets the horizontal alignment of cell content.
    /// Defaults to <see cref="TableColumnAlign.Start"/>.
    /// </summary>
    [Parameter]
    public TableColumnAlign Align { get; set; } = TableColumnAlign.Start;

    /// <summary>
    /// Gets or sets a value indicating whether this column is sortable.
    /// </summary>
    [Parameter]
    public bool Sortable { get; set; }

    /// <summary>
    /// Gets or sets an optional CSS width for the column (e.g. "120px" or "10%").
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets an additional CSS class applied to every cell in this column.
    /// </summary>
    [Parameter]
    public string? CellClass { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        ParentTable?.AddColumn(this);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        ParentTable?.RemoveColumn(this);
        GC.SuppressFinalize(this);
    }
}
