using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// Represents the title element within a modal header.
/// Used within a <see cref="TabModalHeader"/> component for consistent title styling.
/// </summary>
public partial class TabModalTitle : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the content to render as the modal title.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the heading level to use for the title (1-6).
    /// </summary>
    /// <value>The default value is <c>5</c> (renders as &lt;h5&gt;).</value>
    [Parameter]
    public int Level { get; set; } = 5;

    /// <summary>
    /// Gets the HTML tag name based on the heading level.
    /// </summary>
    private string TagName => Level switch
    {
        1 => "h1",
        2 => "h2",
        3 => "h3",
        4 => "h4",
        6 => "h6",
        _ => "h5"
    };

    /// <summary>
    /// Builds a CSS class attribute value by combining the specified base class with any additional CSS classes
    /// defined in the current instance.
    /// </summary>
    /// <returns>A string containing the combined CSS classes, separated by spaces.</returns>
    protected override string BuildCssClass()
    {
        return new CssBuilder("modal-title")
            .AddClass(CssClass)
            .Build();
    }
}
