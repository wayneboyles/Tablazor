namespace Tablazor.Attributes;

/// <summary>
/// Indicates that this property is required when another property is null or empty
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class RequiredIfEmptyAttribute : Attribute
{
    /// <summary>
    /// The name of the property to check
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// The error message to display if validation fails
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequiredIfEmptyAttribute"/> class.
    /// </summary>
    /// <param name="propertyName"></param>
    public RequiredIfEmptyAttribute(string propertyName)
    {
        PropertyName = propertyName;
    }
}