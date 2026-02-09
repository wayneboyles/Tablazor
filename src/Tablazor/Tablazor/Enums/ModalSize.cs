using Tablazor.Attributes;

namespace Tablazor.Enums;

/// <summary>
/// Determines the size of a modal dialog.
/// </summary>
public enum ModalSize
{
    /// <summary>
    /// Default modal size.
    /// </summary>
    Default,

    /// <summary>
    /// Small modal (300px max-width).
    /// </summary>
    [CssClassName("sm")]
    Small,

    /// <summary>
    /// Large modal (800px max-width).
    /// </summary>
    [CssClassName("lg")]
    Large,

    /// <summary>
    /// Full-width modal that spans the entire viewport width.
    /// </summary>
    [CssClassName("full-width")]
    FullWidth
}
