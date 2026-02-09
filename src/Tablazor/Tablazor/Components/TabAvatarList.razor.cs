using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// Represents a list or group of <see cref="TabAvatar"/> components
/// </summary>
public partial class TabAvatarList : TabBaseComponent
{
    /// <summary>
    /// Whether to render the list as stacked Avatars
    /// </summary>
    [Parameter]
    public bool Stacked { get; set; }
    
    /// <summary>
    /// Gets or sets the child content of the list
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
        return new CssBuilder("avatar-list")
            .AddClass("avatar-list-stacked", when: Stacked)
            .Build();
    }
}