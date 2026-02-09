using Tablazor.Attributes;
using Tablazor.Components;

namespace Tablazor.Enums;

/// <summary>
/// Represents the animations available for a <see cref="TabIcon"/>
/// </summary>
public enum IconAnimation
{
    /// <summary>
    /// No animation
    /// </summary>
    Default,
    
    /// <summary>
    /// Pulse animation
    /// </summary>
    [CssClassName("pulse")]
    Pulse,
    
    /// <summary>
    /// Tada animation
    /// </summary>
    [CssClassName("tada")]
    Tada,
    
    /// <summary>
    /// Rotate animation
    /// </summary>
    [CssClassName("rotate")]
    Rotate
}