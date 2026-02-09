using Tablazor.Attributes;

namespace Tablazor.Enums;

/// <summary>
/// Determines the size of the badge
/// </summary>
public enum BadgeSize
{
    /// <summary>
    /// Default badge size
    /// </summary>
    Default,
    
    /// <summary>
    /// Small badge
    /// </summary>
    [CssClassName("sm")]
    Small,
    
    /// <summary>
    /// Large badge
    /// </summary>
    [CssClassName("lg")]
    Large
}