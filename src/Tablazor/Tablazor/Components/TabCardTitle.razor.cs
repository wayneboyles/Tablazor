using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// Represents the title element within a <see cref="TabCardHeader"/>.
/// </summary>
/// <remarks>
/// Renders a heading element (h1-h6) with the card-title CSS class.
/// The heading level can be customized using the <see cref="Level"/> parameter.
/// </remarks>
public partial class TabCardTitle : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the content to render as the card title.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the heading level (1-6) for the title element.
    /// </summary>
    /// <value>The default value is 3, rendering an h3 element.</value>
    /// <remarks>
    /// Values outside the range 1-6 will default to 3.
    /// </remarks>
    [Parameter]
    public int Level { get; set; } = 3;

    /// <summary>
    /// Builds a CSS class attribute value by combining the specified base class with any additional CSS classes
    /// defined in the current instance.
    /// </summary>
    /// <returns>A string containing the combined CSS classes, separated by spaces. If no additional CSS classes are defined,
    /// returns the base class.</returns>
    protected override string BuildCssClass()
    {
        return new CssBuilder("card-title")
            .Build();
    }
}
