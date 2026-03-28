namespace Tablazor.Attributes;

/// <summary>
/// Groups two or more model properties onto the same horizontal form row.
/// All properties sharing the same <see cref="RowId"/> are rendered inside a Bootstrap
/// <c>.row</c> div, each wrapped in a <c>.col-md-{ColumnSpan}</c> column.
/// </summary>
/// <remarks>
/// The column spans of all fields in a row should add up to 12 for a full-width row.
/// <para>Example — first-name / last-name on one line:</para>
/// <code>
/// [SameRow("name", order: 0, columnSpan: 6)]
/// public string FirstName { get; set; }
///
/// [SameRow("name", order: 1, columnSpan: 6)]
/// public string LastName { get; set; }
/// </code>
/// </remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class SameRowAttribute : Attribute
{
    /// <summary>
    /// Shared identifier for the row group.
    /// All properties with the same <see cref="RowId"/> (case-insensitive) are placed
    /// on the same form row.
    /// </summary>
    public string RowId { get; }

    /// <summary>
    /// Display order within the row. Lower values appear further to the left.
    /// Defaults to <c>0</c>.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Bootstrap grid column span (1–12). Defaults to <c>6</c> (half the row width).
    /// All fields in a row should sum to 12 for a full-width layout.
    /// </summary>
    public int ColumnSpan { get; set; } = 6;

    /// <summary>
    /// Marks this property as part of row group <paramref name="rowId"/>.
    /// </summary>
    /// <param name="rowId">The shared row group identifier (case-insensitive).</param>
    /// <param name="order">Column order within the row (default: 0).</param>
    /// <param name="columnSpan">Bootstrap column span 1–12 (default: 6).</param>
    public SameRowAttribute(string rowId, int order = 0, int columnSpan = 6)
    {
        RowId = rowId;
        Order = order;
        ColumnSpan = columnSpan;
    }
}
