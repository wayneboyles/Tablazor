namespace Tablazor.Attributes;

/// <summary>
/// Specifies rows for a textarea
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class TextAreaRowsAttribute : Attribute
{
    public int Rows { get; }

    public TextAreaRowsAttribute(int rows)
    {
        Rows = rows > 0 ? rows : throw new ArgumentException("Rows must be greater than 0", nameof(rows));
    }
}