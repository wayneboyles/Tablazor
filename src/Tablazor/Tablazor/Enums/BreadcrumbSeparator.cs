using Tablazor.Attributes;

namespace Tablazor.Enums;

/// <summary>
/// Specifies the separator style for breadcrumb navigation items.
/// </summary>
public enum BreadcrumbSeparator
{
    /// <summary>
    /// Default separator (forward slash).
    /// </summary>
    [CssClassName("")]
    Default,

    /// <summary>
    /// Chevron/arrow separator (>).
    /// </summary>
    [CssClassName("breadcrumb-arrows")]
    Arrows,

    /// <summary>
    /// Bullet/dot separator.
    /// </summary>
    [CssClassName("breadcrumb-dots")]
    Dots
}
