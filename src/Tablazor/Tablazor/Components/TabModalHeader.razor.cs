using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// Represents the header section of a modal dialog.
/// Used within a <see cref="TabModal"/> component to provide a custom header.
/// </summary>
public partial class TabModalHeader : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the content to render inside the modal header.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show a close button in the header.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    [Parameter]
    public bool ShowCloseButton { get; set; } = true;

    /// <summary>
    /// Gets or sets the callback that is invoked when the close button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnClose { get; set; }

    /// <summary>
    /// Handles the close button click event.
    /// </summary>
    private async Task HandleCloseAsync()
    {
        if (OnClose.HasDelegate)
        {
            await OnClose.InvokeAsync();
        }
    }

    /// <summary>
    /// Builds a CSS class attribute value by combining the specified base class with any additional CSS classes
    /// defined in the current instance.
    /// </summary>
    /// <returns>A string containing the combined CSS classes, separated by spaces.</returns>
    protected override string BuildCssClass()
    {
        return new CssBuilder("modal-header")
            .AddClass(CssClass)
            .Build();
    }
}
