using Tablazor.Attributes;

namespace Tablazor.Enums;

/// <summary>
/// Represents the size of a button
/// </summary>
public enum ButtonSize
{
    /// <summary>
    /// Small button
    /// </summary>
    [CssClassName("sm")]
    Small,
        
    /// <summary>
    /// Default button size
    /// </summary>
    [CssClassName("md")]
    Default,
        
    /// <summary>
    /// Large button
    /// </summary>
    [CssClassName("lg")]
    Large,
        
    /// <summary>
    /// Extra large button
    /// </summary>
    [CssClassName("xl")]
    ExtraLarge
}