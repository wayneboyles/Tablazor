using Tablazor.Enums;

namespace Tablazor.Models;

/// <summary>
/// Represents a single event displayed within a <see cref="Tablazor.Components.TabTimeline"/>.
/// </summary>
public sealed class TimelineItem
{
    /// <summary>
    /// Gets or sets the icon to display in the timeline event marker.
    /// Use a value from <see cref="Tablazor.Icons.TabIcons"/>.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Gets or sets the background color of the icon marker.
    /// </summary>
    public TabColors IconColor { get; set; } = TabColors.Primary;

    /// <summary>
    /// Gets or sets the title of the timeline event.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the message or description of the timeline event.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the display text for when the event occurred (e.g., "Just now", "2 hours ago", "Mar 30").
    /// </summary>
    public string? Time { get; set; }

    /// <summary>
    /// Creates a new <see cref="TimelineItem"/> instance.
    /// </summary>
    public TimelineItem() { }

    /// <summary>
    /// Creates a new <see cref="TimelineItem"/> with the specified properties.
    /// </summary>
    /// <param name="title">The event title.</param>
    /// <param name="message">The event message or description.</param>
    /// <param name="time">The display text for when the event occurred.</param>
    /// <param name="icon">The icon name from <see cref="Tablazor.Icons.TabIcons"/>.</param>
    /// <param name="iconColor">The background color for the icon marker.</param>
    public TimelineItem(string title, string? message = null, string? time = null, string? icon = null, TabColors iconColor = TabColors.Primary)
    {
        Title = title;
        Message = message;
        Time = time;
        Icon = icon;
        IconColor = iconColor;
    }
}