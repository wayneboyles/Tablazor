using Microsoft.AspNetCore.Components;
using Tablazor.Enums;
using Tablazor.Icons;

namespace Tablazor.Components;

/// <summary>
/// Represents a Tabler Icon
/// </summary>
public partial class TabIcon : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the icon to render.
    /// </summary>
    /// <remarks>
    /// Icons should be sourced from <see cref="TabIcons"/> which provide both
    /// outline and filled versions of the icons
    /// </remarks>
    [Parameter]
    [EditorRequired]
    public required string Icon { get; set; }
    
    /// <summary>
    /// Gets or sets the size of the icon.  The default is
    /// 24
    /// </summary>
    [Parameter]
    public int Size { get; set; } = 24;
    
    /// <summary>
    /// Gets or sets the stroke width.  The default
    /// is 2
    /// </summary>
    [Parameter]
    public int StrokeWidth { get; set; } = 2;
    
    /// <summary>
    /// Gets or sets the icon color
    /// </summary>
    [Parameter]
    public TabColors Color { get; set; } = TabColors.Default;
    
    /// <summary>
    /// Gets or sets the icon animation
    /// </summary>
    [Parameter]
    public IconAnimation Animation { get; set; } = IconAnimation.Default;

    /// <summary>
    /// Builds a CSS class attribute value by combining the specified base class with any additional CSS classes
    /// defined in the current instance.
    /// </summary>
    /// <returns>A string containing the combined CSS classes, separated by spaces. If no additional CSS classes are defined,
    /// returns the base class.</returns>
    protected override string BuildCssClass()
    {
        return new CssBuilder("icon")
            .AddClass($"icon-{Animation.GetCssClassName()}", when: Animation != IconAnimation.Default)
            .Build();
    }

    private string GetContainerCssClass()
    {
        return new CssBuilder()
            .AddClass($"text-{Color.GetCssClassName()}", when: Color != TabColors.Default)
            .Build();
    }
}