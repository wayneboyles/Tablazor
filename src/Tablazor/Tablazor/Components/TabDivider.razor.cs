using Microsoft.AspNetCore.Components;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// Dividers help organize content and make the interface layout clear and uncluttered.
/// Greater clarity adds up to better user experience and enhanced interaction with a website or app.
/// </summary>
public partial class TabDivider : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the text to render in the divider
    /// </summary>
    [Parameter]
    [EditorRequired]
    public required string Text { get; set; }
    
    /// <summary>
    /// Gets or sets the text alignment
    /// </summary>
    [Parameter]
    public TextAlignment Alignment { get; set; } = TextAlignment.Default;

    /// <summary>
    /// Gets or sets the color of the text
    /// </summary>
    [Parameter]
    public TabColors Color { get; set; } = TabColors.Default;
    
    /// <summary>
    /// Builds a CSS class attribute value by combining the specified base class with any additional CSS classes
    /// defined in the current instance.
    /// </summary>
    /// <returns>A string containing the combined CSS classes, separated by spaces. If no additional CSS classes are defined,
    /// returns the base class.</returns>
    protected override string BuildCssClass()
    {
        return new CssBuilder("hr-text")
            .AddClass($"hr-text-{Alignment.GetCssClassName()}", when: Alignment != TextAlignment.Default)
            .AddClass($"text-{Color.GetCssClassName()}", when: Color != TabColors.Default)
            .Build();
    }
}