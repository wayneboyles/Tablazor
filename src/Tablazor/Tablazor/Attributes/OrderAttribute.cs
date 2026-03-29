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

    /// <summary>
    /// Creates a new instance of the <see cref="OrderAttribute"/> class
    /// </summary>
    /// <param name="order"></param>
    public OrderAttribute(int order)
    {
        Order = order;
    }
}