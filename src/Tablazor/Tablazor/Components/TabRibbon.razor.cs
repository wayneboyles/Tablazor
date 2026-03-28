using Microsoft.AspNetCore.Components;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// Ribbons are decorative labels that can be attached to a corner or edge of a card or
/// other container element to highlight featured, new, or promoted content.
/// </summary>
/// <remarks>
/// The parent container must have <c>position: relative</c> (or the <c>position-relative</c>
/// CSS class) applied for the ribbon to be positioned correctly.
/// </remarks>
public partial class TabRibbon : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the background color of the ribbon.
    /// When set to a value other than <see cref="TabColors.Default"/>, applies
    /// <c>bg-{color}</c> and <c>text-{color}-fg</c> CSS classes.
    /// </summary>
    [Parameter]
    public TabColors Color { get; set; } = TabColors.Default;

    /// <summary>
    /// Gets or sets the position of the ribbon on its parent element.
    /// Defaults to <see cref="RibbonPosition.TopEnd"/> (top-right corner).
    /// </summary>
    [Parameter]
    public RibbonPosition Position { get; set; } = RibbonPosition.TopEnd;

    /// <summary>
    /// Gets or sets a value indicating whether the ribbon uses the bookmark variant.
    /// When <c>true</c>, the <c>ribbon-bookmark</c> CSS class is applied, rendering
    /// the ribbon with a triangular notch at the bottom.
    /// </summary>
    [Parameter]
    public bool Bookmark { get; set; }

    /// <summary>
    /// Gets or sets the name of a Tabler icon to display inside the ribbon.
    /// When set, a <see cref="TabIcon"/> is rendered before any child content.
    /// Use icon names from <see cref="Tablazor.Icons.TabIcons"/>.
    /// </summary>
    [Parameter]
    public string? Icon { get; set; }

    /// <summary>
    /// Gets or sets the content to render inside the ribbon, such as a text label.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Builds the CSS class string for the ribbon element by combining the base
    /// <c>ribbon</c> class with position, color, and variant modifiers.
    /// </summary>
    /// <returns>
    /// A string containing all applicable CSS classes separated by spaces.
    /// </returns>
    protected override string BuildCssClass()
    {
        return new CssBuilder("ribbon")
            .AddClass(Position.GetCssClassName())
            .AddClass($"bg-{Color.GetCssClassName()}", Color != TabColors.Default)
            .AddClass($"text-{Color.GetCssClassName()}-fg", Color != TabColors.Default)
            .AddClass("ribbon-bookmark", Bookmark)
            .Build();
    }
}
