using Tablazor.Attributes;

namespace Tablazor.Enums;

/// <summary>
/// Determines the animation applied to a placeholder container element.
/// The animation class is applied to the parent wrapper, not the individual placeholder span.
/// </summary>
public enum PlaceholderAnimation
{
    /// <summary>
    /// No animation — the placeholder is static.
    /// </summary>
    None,

    /// <summary>
    /// Glow animation — applies a pulsing opacity effect to the placeholder.
    /// </summary>
    [CssClassName("glow")]
    Glow,

    /// <summary>
    /// Wave animation — applies a left-to-right shimmer wave effect to the placeholder.
    /// </summary>
    [CssClassName("wave")]
    Wave,
}
