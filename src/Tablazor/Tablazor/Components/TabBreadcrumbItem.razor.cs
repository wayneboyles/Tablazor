using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// Represents a single item in a breadcrumb navigation trail.
/// Should be used as a child of <see cref="TabBreadcrumbs"/>.
/// </summary>
public partial class TabBreadcrumbItem : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the display text for the breadcrumb item.
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the URL for the breadcrumb link.
    /// If null or empty, the item will not be rendered as a link.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets an optional icon to display before the text.
    /// Use a value from <see cref="Tablazor.Icons.TabIcons"/>.
    /// </summary>
    [Parameter]
    public string? Icon { get; set; }

    /// <summary>
    /// Gets or sets whether this item represents the current page.
    /// Active items are styled differently and are not rendered as links.
    /// </summary>
    [Parameter]
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the child content to render inside the breadcrumb item.
    /// If provided, this takes precedence over the Text parameter.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a callback invoked when the breadcrumb item is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("breadcrumb-item")
            .AddClass("active", IsActive)
            .AddClass(CssClass)
            .Build();
    }

    /// <summary>
    /// Gets whether the item should render as a link.
    /// </summary>
    protected bool ShouldRenderLink => !IsActive && !string.IsNullOrWhiteSpace(Href);

    private async Task HandleClick()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }
}
