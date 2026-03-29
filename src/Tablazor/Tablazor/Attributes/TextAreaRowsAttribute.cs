namespace Tablazor.Attributes;

/// <summary>
/// Specifies rows for a textarea
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class TextAreaRowsAttribute : Attribute
{
    /// <summary>
    /// The number of rows
    /// </summary>
    public int Rows { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="TextAreaRowsAttribute"/>
    /// class
    /// </summary>
    /// <param name="rows"></param>
    /// <exception cref="ArgumentException"></exception>
    public TextAreaRowsAttribute(int rows)
    {
        Rows = rows > 0 ? rows : throw new ArgumentException("Rows must be greater than 0", nameof(rows));
    }
}