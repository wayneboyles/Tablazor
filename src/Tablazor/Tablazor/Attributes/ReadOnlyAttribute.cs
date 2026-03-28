namespace Tablazor.Attributes;

/// <summary>
/// Marks a field as read-only (disabled in the form)
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ReadOnlyAttribute : Attribute
{
}