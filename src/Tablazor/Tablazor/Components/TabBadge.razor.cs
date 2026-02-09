using Microsoft.AspNetCore.Components;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// Badges are small count and labeling components, which are used to add extra information
/// to an interface element.
/// </summary>
public partial class TabBadge : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the color of the badge.
    /// </summary>
    [Parameter]
    public TabColors Color { get; set; } = TabColors.Default;
    
    /// <summary>
    /// Gets or sets the shape of the badge.
    /// </summary>
    [Parameter]
    public BadgeShape Shape { get; set; } = BadgeShape.Default;
    
    /// <summary>
    /// Gets or sets the size of the badge
    /// </summary>
    [Parameter]
    public BadgeSize Size { get; set; } =  BadgeSize.Default;
    
    /// <summary>
    /// Gets or sets the icon to display in the badge
    /// </summary>
    [Parameter]
    public string? Icon { get; set; }
    
    /// <summary>
    /// Gets or sets a URL for the badge.  When set, the badge will render
    /// a link.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }
    
    /// <summary>
    /// Gets or sets the content to render in the badge.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Builds a CSS class attribute value by combining the specified base class with any additional CSS classes
    /// defined in the current instance.
    /// </summary>
    /// <returns>A string containing the combined CSS classes, separated by spaces. If no additional CSS classes are defined,
    /// returns the base class.</returns>
    protected override string BuildCssClass()
    {
        return new CssBuilder("badge")
            .AddClass($"bg-{Color.GetCssClassName()}", Color != TabColors.Default)
            .AddClass($"text-{Color.GetCssClassName()}-fg", Color != TabColors.Default)
            .AddClass("badge-pill", Shape == BadgeShape.Pill)
            .AddClass($"badge-{Size.GetCssClassName()}", Size != BadgeSize.Default)
            .Build();
    }
}