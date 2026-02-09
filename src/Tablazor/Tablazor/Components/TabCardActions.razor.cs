using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// Represents the actions container within a <see cref="TabCardHeader"/>.
/// </summary>
/// <remarks>
/// Place this component within a <see cref="TabCardHeader"/> to create an area for
/// action buttons, dropdowns, or other interactive elements. The actions are
/// automatically aligned to the end (right in LTR layouts) of the header.
/// </remarks>
public partial class TabCardActions : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the content to render as card actions.
    /// </summary>
    /// <remarks>
    /// Typically contains buttons, links, or dropdown menus.
    /// </remarks>
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
        return new CssBuilder("card-actions")
            .Build();
    }
}
