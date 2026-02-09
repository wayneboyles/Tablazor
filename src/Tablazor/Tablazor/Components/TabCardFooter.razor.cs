using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// Represents the footer section of a <see cref="TabCard"/>.
/// </summary>
/// <remarks>
/// Place this component as a direct child of <see cref="TabCard"/> to create a styled
/// footer area. Typically used for action buttons, links, or supplementary information.
/// </remarks>
public partial class TabCardFooter : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the content to render inside the card footer.
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
        return new CssBuilder("card-footer")
            .Build();
    }
}
