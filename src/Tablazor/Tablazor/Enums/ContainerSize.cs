using Tablazor.Attributes;

namespace Tablazor.Enums;

/// <summary>
/// Determines the container width for layout components such as
/// <see cref="Tablazor.Components.TabNavbar"/>, <see cref="Tablazor.Components.TabPageBody"/>,
/// and <see cref="Tablazor.Components.TabFooter"/>.
/// </summary>
public enum ContainerSize
{
    /// <summary>
    /// Fluid container — full width at all viewport sizes.
    /// Renders as <c>container-fluid</c>.
    /// </summary>
    [CssClassName("fluid")]
    Fluid,

    /// <summary>
    /// Small container — max-width breakpoint at <c>sm</c>.
    /// Renders as <c>container-sm</c>.
    /// </summary>
    [CssClassName("sm")]
    Small,

    /// <summary>
    /// Medium container — max-width breakpoint at <c>md</c>.
    /// Renders as <c>container-md</c>.
    /// </summary>
    [CssClassName("md")]
    Medium,

    /// <summary>
    /// Large container — max-width breakpoint at <c>lg</c>.
    /// Renders as <c>container-lg</c>.
    /// </summary>
    [CssClassName("lg")]
    Large,

    /// <summary>
    /// Extra-large container — max-width breakpoint at <c>xl</c>.
    /// This is the default for most Tablazor layout components.
    /// Renders as <c>container-xl</c>.
    /// </summary>
    [CssClassName("xl")]
    ExtraLarge,

    /// <summary>
    /// Extra-extra-large container — max-width breakpoint at <c>xxl</c>.
    /// Renders as <c>container-xxl</c>.
    /// </summary>
    [CssClassName("xxl")]
    ExtraExtraLarge,
}
