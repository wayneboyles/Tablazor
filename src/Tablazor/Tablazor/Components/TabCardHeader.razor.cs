using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// Represents the header section of a <see cref="TabCard"/>.
/// </summary>
/// <remarks>
/// Place this component as a direct child of <see cref="TabCard"/> to create a styled
/// header area. Typically contains <see cref="TabCardTitle"/>, <see cref="TabCardSubtitle"/>,
/// or <see cref="TabCardActions"/> components.
/// </remarks>
public partial class TabCardHeader : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the content to render inside the card header.
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
        return new CssBuilder("card-header")
            .Build();
    }
}
