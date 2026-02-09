using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// Represents the body section of a modal dialog.
/// Used within a <see cref="TabModal"/> component to contain the main content.
/// </summary>
public partial class TabModalBody : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the content to render inside the modal body.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Builds a CSS class attribute value by combining the specified base class with any additional CSS classes
    /// defined in the current instance.
    /// </summary>
    /// <returns>A string containing the combined CSS classes, separated by spaces.</returns>
    protected override string BuildCssClass()
    {
        return new CssBuilder("modal-body")
            .AddClass(CssClass)
            .Build();
    }
}
