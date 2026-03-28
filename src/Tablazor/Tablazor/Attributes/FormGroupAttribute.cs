namespace Tablazor.Attributes;

/// <summary>
/// Groups properties into sections within the form
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FormGroupAttribute : Attribute
{
    /// <summary>
    /// The name of the form group
    /// </summary>
    public string GroupName { get; set; }
    
    /// <summary>
    /// The order of the group
    /// </summary>
    public int Order { get; set; }

    public FormGroupAttribute(string groupName)
    {
        GroupName = groupName ?? throw new ArgumentNullException(nameof(groupName));
    }
}