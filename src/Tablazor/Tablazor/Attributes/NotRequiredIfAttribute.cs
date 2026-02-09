namespace Tablazor.Attributes;

/// <summary>
/// Specifies that a parameter must not be set if another parameter has a specific value.
/// This is used to enforce mutual exclusivity between parameters.
/// </summary>
/// <example>
/// <code>
/// [Parameter]
/// [NotRequiredIf(nameof(Param2), errorIfSet: true)]
/// public string? Param1 { get; set; }
/// 
/// [Parameter]
/// public string? Param2 { get; set; }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class NotRequiredIfAttribute : Attribute
{
    /// <summary>
    /// Gets the name of the property that this parameter conflicts with.
    /// </summary>
    public string ConflictingProperty { get; }

    /// <summary>
    /// Gets the value that the conflicting property must have for this validation to apply.
    /// If null, any non-null value triggers the validation.
    /// </summary>
    public object? ConflictingValue { get; }

    /// <summary>
    /// Gets a value indicating whether an error should be raised if this property is set
    /// when the conflicting condition is met.
    /// </summary>
    public bool ErrorIfSet { get; }

    /// <summary>
    /// Gets or sets a custom error message to display when validation fails.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotRequiredIfAttribute"/> class
    /// for basic mutual exclusivity (any value conflicts).
    /// </summary>
    /// <param name="conflictingProperty">The name of the property that conflicts with this one.</param>
    /// <param name="errorIfSet">If true, raises an error when both properties are set.</param>
    public NotRequiredIfAttribute(string conflictingProperty, bool errorIfSet = true)
        : this(conflictingProperty, null, errorIfSet)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotRequiredIfAttribute"/> class
    /// for value-specific conflicts.
    /// </summary>
    /// <param name="conflictingProperty">The name of the property that conflicts with this one.</param>
    /// <param name="conflictingValue">The specific value that causes a conflict.</param>
    /// <param name="errorIfSet">If true, raises an error when both conditions are met.</param>
    public NotRequiredIfAttribute(string conflictingProperty, object? conflictingValue, bool errorIfSet = true)
    {
        if (string.IsNullOrWhiteSpace(conflictingProperty))
        {
            throw new ArgumentException("Conflicting property name cannot be null or whitespace.", nameof(conflictingProperty));
        }

        ConflictingProperty = conflictingProperty;
        ConflictingValue = conflictingValue;
        ErrorIfSet = errorIfSet;
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

        if (ConflictingValue == null)
        {
            return $"Parameter '{propertyName}' cannot be set when '{ConflictingProperty}' is set.";
        }

        var valueDisplay = ConflictingValue.ToString();
        return $"Parameter '{propertyName}' cannot be set when '{ConflictingProperty}' is set to '{valueDisplay}'.";
    }
}