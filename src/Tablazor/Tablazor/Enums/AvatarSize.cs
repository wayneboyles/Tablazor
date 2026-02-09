using Tablazor.Attributes;
using Tablazor.Components;

namespace Tablazor.Enums;

/// <summary>
/// Represents the size of a <see cref="TabAvatar"/> component
/// </summary>
public enum AvatarSize
{
    /// <summary>
    /// Default avatar size
    /// </summary>
    Default,
    
    /// <summary>
    /// Small avatar
    /// </summary>
    [CssClassName("sm")]
    Small,
    
    /// <summary>
    /// Extra small avatar
    /// </summary>
    [CssClassName("xs")]
    ExtraSmall,
    
    /// <summary>
    /// Large avatar
    /// </summary>
    [CssClassName("lg")]
    Large,
    
    /// <summary>
    /// Extra large avatar
    /// </summary>
    [CssClassName("xl")]
    ExtraLarge
}