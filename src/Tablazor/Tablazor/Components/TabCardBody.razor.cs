using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// Represents the body section of a <see cref="TabCard"/>.
/// </summary>
/// <remarks>
/// Place this component as a direct child of <see cref="TabCard"/> to create the main
/// content area of the card. Multiple <see cref="TabCardBody"/> components can be used
/// within a single card to create distinct content sections.
/// </remarks>
public partial class TabCardBody : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the content to render inside the card body.
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
        return new CssBuilder("card-body")
            .Build();
    }
}
