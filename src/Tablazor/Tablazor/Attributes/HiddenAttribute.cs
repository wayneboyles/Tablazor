namespace Tablazor.Attributes;

/// <summary>
/// Hides a property from the dynamic form while still allowing it to be bound
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class HiddenAttribute : Attribute
{
}