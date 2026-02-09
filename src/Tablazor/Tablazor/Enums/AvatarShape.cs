using Tablazor.Attributes;
using Tablazor.Components;

namespace Tablazor.Enums;

/// <summary>
/// Determines the shape of a <see cref="TabAvatar"/>
/// </summary>
public enum AvatarShape
{
    /// <summary>
    /// Default Avatar shape
    /// </summary>
    Default,
    
    /// <summary>
    /// Rounded Avatar
    /// </summary>
    [CssClassName("rounded")]
    Rounded,
    
    /// <summary>
    /// Circle Avatar
    /// </summary>
    [CssClassName("rounded-circle")]
    Circle,
    
    /// <summary>
    /// Square Avatar
    /// </summary>
    [CssClassName("rounded-0")]
    Square
}