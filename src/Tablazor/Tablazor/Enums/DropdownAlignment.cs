using Tablazor.Attributes;

namespace Tablazor.Enums;

/// <summary>
/// Specifies the alignment of a dropdown menu relative to its toggle element.
/// </summary>
public enum DropdownAlignment
{
    /// <summary>
    /// Aligns the dropdown menu to the start (left in LTR layouts). This is the default.
    /// </summary>
    [CssClassName("dropdown-menu-start")]
    Start,

    /// <summary>
    /// Aligns the dropdown menu to the end (right in LTR layouts).
    /// </summary>
    [CssClassName("dropdown-menu-end")]
    End
}
