namespace Tablazor.Attributes;

/// <summary>
/// Specifies that a field should render in full width (don't share row with other fields)
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FullWidthAttribute : Attribute
{
}