using Microsoft.AspNetCore.Components;
using Tablazor.Enums;
using Tablazor.Models;

namespace Tablazor.Components;

/// <summary>
/// Displays a vertical timeline of events, each with an icon, title, message, and time.
/// </summary>
public partial class TabTimeline : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the collection of timeline events to display.
    /// </summary>
    [Parameter]
    public IEnumerable<TimelineItem> Items { get; set; } = [];

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("timeline")
            .AddClass(CssClass)
            .Build();
    }

    private static string GetIconBgClass(TimelineItem item)
    {
        return new CssBuilder()
            .AddClass($"bg-{item.IconColor.GetCssClassName()}-lt", item.IconColor != TabColors.Default)
            .Build();
    }
}