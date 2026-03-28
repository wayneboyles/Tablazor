namespace Tablazor.Attributes;

/// <summary>
/// Provides a placeholder text for input fields
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class PlaceholderAttribute : Attribute
{
    /// <summary>
    /// The placeholder text
    /// </summary>
    public string Text { get; }

    public PlaceholderAttribute(string text)
    {
        Text = text ?? throw new ArgumentNullException(nameof(text));
    }
}