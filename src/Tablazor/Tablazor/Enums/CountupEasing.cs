using Tablazor.Attributes;

namespace Tablazor.Enums;

/// <summary>
/// Represents the easing function to use for countup animations.
/// </summary>
public enum CountupEasing
{
    /// <summary>
    /// No easing - linear animation.
    /// </summary>
    [CssClassName("linear")]
    Linear,

    /// <summary>
    /// Starts slow and accelerates (quadratic).
    /// </summary>
    [CssClassName("ease-in")]
    EaseIn,

    /// <summary>
    /// Starts fast and decelerates (quadratic).
    /// </summary>
    [CssClassName("ease-out")]
    EaseOut,

    /// <summary>
    /// Starts slow, accelerates, then decelerates (quadratic).
    /// </summary>
    [CssClassName("ease-in-out")]
    EaseInOut,

    /// <summary>
    /// Exponential ease out - the default countup.js easing.
    /// Starts very fast and decelerates dramatically.
    /// </summary>
    [CssClassName("ease-out-expo")]
    EaseOutExpo,

    /// <summary>
    /// Cubic ease out - smooth deceleration.
    /// </summary>
    [CssClassName("ease-out-cubic")]
    EaseOutCubic,

    /// <summary>
    /// Elastic ease out - overshoots then settles.
    /// </summary>
    [CssClassName("ease-out-elastic")]
    EaseOutElastic,

    /// <summary>
    /// Bounce ease out - bounces at the end.
    /// </summary>
    [CssClassName("ease-out-bounce")]
    EaseOutBounce
}
