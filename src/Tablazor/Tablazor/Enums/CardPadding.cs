using Tablazor.Attributes;
using Tablazor.Components;

namespace Tablazor.Enums;

/// <summary>
/// Determines the padding used for a <see cref="TabCard"/>
/// </summary>
public enum CardPadding
{
    /// <summary>
    /// Default padding
    /// </summary>
    Default,
    
    /// <summary>
    /// Small padding
    /// </summary>
    [CssClassName("sm")]
    Small,
    
    /// <summary>
    /// Medium padding
    /// </summary>
    [CssClassName("md")]
    Medium,
    
    /// <summary>
    /// Large padding
    /// </summary>
    [CssClassName("lg")]
    Large
}