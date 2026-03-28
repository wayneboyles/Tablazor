namespace Tablazor.Attributes;

/// <summary>
/// Associates a field with a conditional visibility rule
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ConditionalVisibilityAttribute : Attribute
{
    /// <summary>
    /// The property that the condition depends on
    /// </summary>
    public string DependsOnProperty { get; }
    
    /// <summary>
    /// The required value of the dependent property
    /// </summary>
    public object? RequiredValue { get; }

    /// <summary>
    /// Field is shown only if DependsOnProperty equals RequiredValue
    /// </summary>
    public ConditionalVisibilityAttribute(string dependsOnProperty, object? requiredValue = null)
    {
        DependsOnProperty = dependsOnProperty ?? throw new ArgumentNullException(nameof(dependsOnProperty));
        RequiredValue = requiredValue;
    }
}