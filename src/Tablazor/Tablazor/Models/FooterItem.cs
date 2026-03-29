using Tablazor.Components;

namespace Tablazor.Models;

/// <summary>
/// Represents a link or text displayed within the
/// <see cref="TabFooter"/> component
/// </summary>
public sealed class FooterItem
{
    /// <summary>
    /// Gets or sets the text of the footer item
    /// </summary>
    public string Text { get; set; }
    
    /// <summary>
    /// Gets or sets the URL of the footer item
    /// </summary>
    public string? Url { get; set; }
}