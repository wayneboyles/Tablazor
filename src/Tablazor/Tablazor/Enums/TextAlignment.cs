using Tablazor.Attributes;

namespace Tablazor.Enums;

/// <summary>
/// Determines the alignment of text
/// </summary>
public enum TextAlignment
{
    /// <summary>
    /// Default alignment
    /// </summary>
    Default,
    
    /// <summary>
    /// Left aligned
    /// </summary>
    [CssClassName("start")]
    Left,
    
    /// <summary>
    /// Right aligned
    /// </summary>
    [CssClassName("end")]
    Right
}