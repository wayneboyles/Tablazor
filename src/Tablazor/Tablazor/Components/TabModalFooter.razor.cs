using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// Represents the footer section of a modal dialog.
/// Used within a <see cref="TabModal"/> component to contain action buttons.
/// </summary>
public partial class TabModalFooter : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the content to render inside the modal footer.
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
        return new CssBuilder("modal-footer")
            .AddClass(CssClass)
            .Build();
    }
}
