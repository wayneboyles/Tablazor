using Microsoft.AspNetCore.Components;
using Tablazor.Attributes;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// Avatars help customize various elements of a user interface and make the
/// product experience more personalized
/// </summary>
public partial class TabAvatar : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the image URL for the avatar
    /// </summary>
    [Parameter]
    [NotRequiredIf(nameof(Icon))]
    [NotRequiredIf(nameof(ChildContent))]
    public string? ImageUrl { get; set; }
    
    /// <summary>
    /// Gets or sets the avatar size
    /// </summary>
    [Parameter]
    public AvatarSize Size { get; set; } = AvatarSize.Default;
    
    /// <summary>
    /// Gets or sets the color of the avatar
    /// </summary>
    [Parameter]
    [NotRequiredIf(nameof(ImageUrl))]
    public TabColors Color { get; set; } = TabColors.Default;
    
    /// <summary>
    /// Gets or sets the content to render within the avatar
    /// </summary>
    [Parameter]
    [NotRequiredIf(nameof(ImageUrl))]
    [NotRequiredIf(nameof(Icon))]
    public RenderFragment? ChildContent { get; set; }
    
    /// <summary>
    /// Gets or sets the icon to display as an avatar
    /// </summary>
    [Parameter]
    [NotRequiredIf(nameof(ImageUrl))]
    [NotRequiredIf(nameof(ChildContent))]
    public string? Icon { get; set; }
    
    /// <summary>
    /// Gets or sets the shape of the avatar
    /// </summary>
    [Parameter] 
    public AvatarShape Shape { get; set; } = AvatarShape.Default;
    
    /// <summary>
    /// Gets or sets the color of a status indicator
    /// </summary>
    [Parameter]
    public TabColors StatusColor { get; set; } = TabColors.Default;

    /// <summary>
    /// Builds a CSS class attribute value by combining the specified base class with any additional CSS classes
    /// defined in the current instance.
    /// </summary>
    /// <returns>A string containing the combined CSS classes, separated by spaces. If no additional CSS classes are defined,
    /// returns the base class.</returns>
    protected override string BuildCssClass()
    {
        return new CssBuilder("avatar")
            .AddClass($"avatar-{Size.GetCssClassName()}", when: Size != AvatarSize.Default)
            .AddClass($"bg-{Color.GetCssClassName()}-lt", when: Color != TabColors.Default)
            .AddClass(Shape.GetCssClassName(), when: Shape != AvatarShape.Default)
            .Build();
    }

    /// <summary>
    /// Builds a CSS style string by combinding the specified base class with any additional
    /// Style attributes defined in the current instance
    /// </summary>
    /// <returns>A string containing the combined Style</returns>
    protected override string BuildStyleString()
    {
        return new StyleBuilder()
            .AddStyle("background-image", $"url({ImageUrl})", when: !string.IsNullOrWhiteSpace(ImageUrl))
            .Build();
    }

    private string? GetStatusCssClass()
    {
        return new CssBuilder("badge")
            .AddClass($"bg-{StatusColor.GetCssClassName()}", when: StatusColor != TabColors.Default)
            .Build();
    }
}