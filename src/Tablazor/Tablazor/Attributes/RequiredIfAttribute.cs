namespace Tablazor.Attributes;

/// <summary>
/// Specifies that a parameter is required if another parameter has a specific value.
/// </summary>
/// <example>
/// <code>
/// [Parameter]
/// [RequiredIf(nameof(EnableFeature), true)]
/// public string? FeatureConfig { get; set; }
/// 
/// [Parameter]
/// public bool EnableFeature { get; set; }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class RequiredIfAttribute : Attribute
{
    /// <summary>
    /// Gets the name of the property that this parameter depends on.
    /// </summary>
    public string DependentProperty { get; }

    /// <summary>
    /// Gets the value that the dependent property must have for this parameter to be required.
    /// </summary>
    public object? DependentValue { get; }

    /// <summary>
    /// Gets or sets a custom error message to display when validation fails.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequiredIfAttribute"/> class.
    /// </summary>
    /// <param name="dependentProperty">The name of the property this parameter depends on.</param>
    /// <param name="dependentValue">The value that the dependent property must have.</param>
    public RequiredIfAttribute(string dependentProperty, object? dependentValue)
    {
        if (string.IsNullOrWhiteSpace(dependentProperty))
        {
            throw new ArgumentException("Dependent property name cannot be null or whitespace.", nameof(dependentProperty));
        }

        DependentProperty = dependentProperty;
        DependentValue = dependentValue;
    }

    /// <summary>
    /// Gets the formatted error message for validation failures.
    /// </summary>
    /// <param name="propertyName">The name of the property being validated.</param>
    /// <returns>The error message.</returns>
    public string GetErrorMessage(string propertyName)
    {
        if (!string.IsNullOrWhiteSpace(ErrorMessage))
        {
            return ErrorMessage;
        }

        var valueDisplay = DependentValue?.ToString() ?? "null";
        return $"Parameter '{propertyName}' is required when '{DependentProperty}' is set to '{valueDisplay}'.";
    }
}