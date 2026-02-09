using Tablazor.Attributes;
using Tablazor.Components;

namespace Tablazor.Enums;

/// <summary>
/// Determines the position of the status border on a <see cref="TabCard"/>.
/// </summary>
public enum CardStatusPosition
{
    /// <summary>
    /// No status border is displayed.
    /// </summary>
    None,

    /// <summary>
    /// Status border is displayed at the top of the card.
    /// </summary>
    [CssClassName("top")]
    Top,

    /// <summary>
    /// Status border is displayed at the start (left in LTR, right in RTL) of the card.
    /// </summary>
    [CssClassName("start")]
    Start
}
