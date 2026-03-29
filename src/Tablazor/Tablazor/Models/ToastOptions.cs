using Microsoft.AspNetCore.Components;

using Tablazor.Enums;

namespace Tablazor.Models;

/// <summary>
/// Represents the configuration options for a toast notification.
/// </summary>
public sealed class ToastOptions
{
    /// <summary>
    /// Gets or sets the title/header text of the toast.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the message body of the toast.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the color theme of the toast.
    /// </summary>
    public TabColors Color { get; set; } = TabColors.Default;

    /// <summary>
    /// Gets or sets the icon to display in the toast header.
    /// This should be an SVG string or icon markup.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the toast can be manually closed by the user.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    public bool Closable { get; set; } = true;

    /// <summary>
    /// Gets or sets the duration in milliseconds before the toast automatically closes.
    /// Set to 0 or negative to disable auto-close.
    /// </summary>
    /// <value>The default value is 5000 milliseconds (5 seconds).</value>
    public int AutoCloseDelay { get; set; } = 5000;

    /// <summary>
    /// Gets or sets a value indicating whether the toast should have a translucent background.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    public bool Translucent { get; set; }

    /// <summary>
    /// Gets or sets custom content to render in the toast body.
    /// When set, this takes precedence over <see cref="Message"/>.
    /// </summary>
    public RenderFragment? Content { get; set; }

    /// <summary>
    /// Gets or sets a callback that is invoked when the toast is closed.
    /// </summary>
    public Action? OnClose { get; set; }

    /// <summary>
    /// Gets or sets additional CSS classes to apply to the toast.
    /// </summary>
    public string? CssClass { get; set; }
}