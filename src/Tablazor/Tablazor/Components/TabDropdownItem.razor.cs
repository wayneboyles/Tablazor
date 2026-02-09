using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Tablazor.Components;

/// <summary>
/// Represents an individual item within a <see cref="TabDropdownMenu"/>.
/// Can be rendered as a button or an anchor element depending on whether
/// <see cref="Href"/> is provided.
/// </summary>
public partial class TabDropdownItem : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the child content of the dropdown item.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the URL for the dropdown item. When set, the item renders as an anchor element.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the target attribute for the anchor element.
    /// Only applicable when <see cref="Href"/> is set.
    /// </summary>
    [Parameter]
    public string? Target { get; set; }

    /// <summary>
    /// Gets or sets the icon to display in the dropdown item.
    /// </summary>
    /// <remarks>
    /// Icons should be sourced from <see cref="Tablazor.Icons.TabIcons"/> which provides
    /// both outline and filled versions of icons.
    /// </remarks>
    [Parameter]
    public string? Icon { get; set; }

    /// <summary>
    /// Gets or sets whether the item is in an active state.
    /// </summary>
    [Parameter]
    public bool Active { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the dropdown item is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Gets or sets the parent dropdown component.
    /// </summary>
    [CascadingParameter]
    internal TabDropdown? ParentDropdown { get; set; }

    /// <summary>
    /// Handles the click event for the dropdown item.
    /// </summary>
    /// <param name="args">The mouse event arguments.</param>
    private async Task HandleClickAsync(MouseEventArgs args)
    {
        if (Disabled)
        {
            return;
        }

        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(args);
        }
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("dropdown-item")
            .AddClass("active", Active)
            .AddClass("disabled", Disabled)
            .AddClass(CssClass)
            .Build();
    }
}
