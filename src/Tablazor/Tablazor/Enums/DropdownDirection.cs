using Tablazor.Attributes;

namespace Tablazor.Enums;

/// <summary>
/// Specifies the direction in which a dropdown menu opens.
/// </summary>
public enum DropdownDirection
{
    /// <summary>
    /// The dropdown opens downward. This is the default.
    /// </summary>
    [CssClassName("dropdown")]
    Down,

    /// <summary>
    /// The dropdown opens upward.
    /// </summary>
    [CssClassName("dropup")]
    Up,

    /// <summary>
    /// The dropdown opens to the start (left in LTR layouts).
    /// </summary>
    [CssClassName("dropstart")]
    Start,

    /// <summary>
    /// The dropdown opens to the end (right in LTR layouts).
    /// </summary>
    [CssClassName("dropend")]
    End
}
