using Microsoft.AspNetCore.Components;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// Renders a wrapper element that applies a placeholder animation and groups
/// one or more <see cref="TabPlaceholder"/> child elements together.
/// </summary>
/// <remarks>
/// Per the Bootstrap/Tabler placeholder pattern, the animation class (<c>placeholder-glow</c>
/// or <c>placeholder-wave</c>) is applied to the container element, not to the individual
/// placeholder spans. Set <c>aria-hidden="true"</c> (the default) when using this as a
/// loading skeleton that screen readers should ignore.
/// </remarks>
public partial class TabPlaceholderContainer : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the animation applied to the placeholder container.
    /// When set to a value other than <see cref="PlaceholderAnimation.None"/>, applies
    /// <c>placeholder-glow</c> or <c>placeholder-wave</c> to the wrapper element.
    /// </summary>
    [Parameter]
    public PlaceholderAnimation Animation { get; set; } = PlaceholderAnimation.None;

    /// <summary>
    /// Gets or sets the HTML tag used to render the container element.
    /// Supported values are <c>"div"</c>, <c>"p"</c>, and <c>"span"</c>.
    /// Defaults to <c>"div"</c>.
    /// </summary>
    [Parameter]
    public string Tag { get; set; } = "div";

    /// <summary>
    /// Gets or sets the child content to render inside the container.
    /// Typically one or more <see cref="TabPlaceholder"/> components.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the <c>aria-hidden="true"</c> attribute
    /// is applied to the container element. Defaults to <c>true</c>, which hides the
    /// skeleton from assistive technologies during loading states.
    /// </summary>
    [Parameter]
    public bool AriaHidden { get; set; } = true;

    /// <summary>
    /// Builds the CSS class string for the container element.
    /// Includes an animation class when <see cref="Animation"/> is not <see cref="PlaceholderAnimation.None"/>.
    /// </summary>
    /// <returns>A string containing all applicable CSS classes separated by spaces.</returns>
    protected override string BuildCssClass()
    {
        return new CssBuilder()
            .AddClass($"placeholder-{Animation.GetCssClassName()}", Animation != PlaceholderAnimation.None)
            .Build();
    }
}
