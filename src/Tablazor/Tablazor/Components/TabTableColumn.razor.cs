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
    /// Gets or sets a custom cell template. When set, takes priority over all field convenience
    /// parameters and renders the template for each row's item.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? CellTemplate { get; set; }

    /// <summary>
    /// Gets or sets a delegate used exclusively for sorting when <see cref="Sortable"/> is
    /// <c>true</c> and the column uses <see cref="CellTemplate"/> or another non-text renderer.
    /// When <c>null</c>, <see cref="Field"/> is used as the sort key.
    /// </summary>
    [Parameter]
    public Func<TItem, object?>? SortField { get; set; }

    /// <summary>
    /// Gets or sets a delegate that returns SVG path data (from <see cref="Tablazor.Icons.TabIcons"/>)
    /// or a CSS class name for the icon to display in the cell via <see cref="TabIcon"/>.
    /// </summary>
    [Parameter]
    public Func<TItem, string?>? IconField { get; set; }

    /// <summary>
    /// Gets or sets a delegate that returns an image URL to display as a <see cref="TabAvatar"/>
    /// in the cell. Use <see cref="AvatarColor"/> to set the fallback background color.
    /// </summary>
    [Parameter]
    public Func<TItem, string?>? AvatarField { get; set; }

    /// <summary>
    /// Gets or sets the color applied to the <see cref="TabAvatar"/> rendered by <see cref="AvatarField"/>.
    /// Defaults to <see cref="TabColors.Default"/>.
    /// </summary>
    [Parameter]
    public TabColors AvatarColor { get; set; } = TabColors.Default;

    /// <summary>
    /// Gets or sets a delegate that returns an image URL to render as a plain <c>&lt;img&gt;</c>
    /// element in the cell.
    /// </summary>
    [Parameter]
    public Func<TItem, string?>? ImageField { get; set; }

    /// <summary>
    /// Gets or sets the alt text used when rendering images via <see cref="ImageField"/>.
    /// Defaults to an empty string.
    /// </summary>
    [Parameter]
    public string ImageAlt { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the CSS class applied to images rendered via <see cref="ImageField"/>.
    /// </summary>
    [Parameter]
    public string? ImageClass { get; set; }

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
