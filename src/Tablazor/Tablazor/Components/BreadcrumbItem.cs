namespace Tablazor.Components;

/// <summary>
/// Represents a single breadcrumb navigation item.
/// </summary>
public class BreadcrumbItem
{
    /// <summary>
    /// Gets or sets the display text for the breadcrumb item.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL for the breadcrumb item.
    /// If null or empty, the item will not be rendered as a link.
    /// </summary>
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets an optional icon to display before the text.
    /// Use a value from <see cref="Tablazor.Icons.TabIcons"/>.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Gets or sets whether this item is the active (current) page.
    /// Active items are not rendered as links.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Creates a new instance of <see cref="BreadcrumbItem"/>.
    /// </summary>
    public BreadcrumbItem()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="BreadcrumbItem"/> with the specified text and href.
    /// </summary>
    /// <param name="text">The display text for the breadcrumb.</param>
    /// <param name="href">The URL for the breadcrumb link.</param>
    /// <param name="isActive">Whether this item is the current page.</param>
    public BreadcrumbItem(string text, string? href = null, bool isActive = false)
    {
        Text = text;
        Href = href;
        IsActive = isActive;
    }

    /// <summary>
    /// Creates a new instance of <see cref="BreadcrumbItem"/> with the specified text, href, and icon.
    /// </summary>
    /// <param name="text">The display text for the breadcrumb.</param>
    /// <param name="href">The URL for the breadcrumb link.</param>
    /// <param name="icon">The icon to display before the text.</param>
    /// <param name="isActive">Whether this item is the current page.</param>
    public BreadcrumbItem(string text, string? href, string? icon, bool isActive = false)
    {
        Text = text;
        Href = href;
        Icon = icon;
        IsActive = isActive;
    }
}
