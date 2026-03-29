namespace Tablazor.Models;

/// <summary>
/// Represents a single rendered row in the form.
/// A row contains one field (single-column) or several fields that share a
/// <see cref="Attributes.SameRowAttribute.RowId"/> (multi-column).
/// </summary>
internal sealed class FormRow
{
    /// <summary>
    /// The shared <see cref="Attributes.SameRowAttribute.RowId"/> for multi-column rows.
    /// <c>null</c> for single-field rows.
    /// </summary>
    public string? RowId { get; init; }

    /// <summary>
    /// Ordered list of fields to render within this row.
    /// Contains exactly one entry for single-field rows.
    /// </summary>
    public List<FieldDescriptor> Fields { get; init; } = new();

    /// <summary>
    /// <c>true</c> when two or more fields share this row.
    /// </summary>
    public bool IsMultiColumn => Fields.Count > 1;
}