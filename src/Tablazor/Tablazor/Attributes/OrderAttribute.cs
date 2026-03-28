namespace Tablazor.Attributes;

/// <summary>
/// Specifies the order in which fields should appear
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class OrderAttribute : Attribute
{
    /// <summary>
    /// The field order
    /// </summary>
    public int Order { get; }

    public OrderAttribute(int order)
    {
        Order = order;
    }
}