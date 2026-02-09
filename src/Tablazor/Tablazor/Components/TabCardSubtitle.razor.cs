using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// Represents the subtitle element within a <see cref="TabCardHeader"/>.
/// </summary>
/// <remarks>
/// Renders a paragraph element with the card-subtitle CSS class.
/// Typically placed after a <see cref="TabCardTitle"/> to provide additional context.
/// </remarks>
public partial class TabCardSubtitle : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the content to render as the card subtitle.
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
        return new CssBuilder("card-subtitle")
            .Build();
    }
}
