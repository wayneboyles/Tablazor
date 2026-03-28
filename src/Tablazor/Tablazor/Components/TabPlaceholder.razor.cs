using Microsoft.AspNetCore.Components;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// Renders a single placeholder span element used to represent a loading skeleton.
/// Placeholders are purely CSS-based skeleton loaders that indicate content is being loaded.
/// </summary>
/// <remarks>
/// Use <see cref="TabPlaceholderContainer"/> as the parent wrapper to apply animation
/// (glow or wave) to a group of placeholders.
/// </remarks>
public partial class TabPlaceholder : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the number of Bootstrap grid columns the placeholder spans.
    /// Accepts values from 1 to 12. Values outside this range are clamped.
    /// Only applied when <see cref="Width"/> is not set.
    /// </summary>
    [Parameter]
    public int Columns { get; set; } = 12;

    /// <summary>
    /// Gets or sets the size modifier of the placeholder.
    /// When set to a value other than <see cref="PlaceholderSize.Default"/>, applies
    /// a <c>placeholder-xs</c>, <c>placeholder-sm</c>, or <c>placeholder-lg</c> CSS class.
    /// </summary>
    [Parameter]
    public PlaceholderSize Size { get; set; } = PlaceholderSize.Default;

    /// <summary>
    /// Gets or sets the background color of the placeholder.
    /// When set to a value other than <see cref="TabColors.Default"/>, applies a <c>bg-{color}</c> CSS class.
    /// </summary>
    [Parameter]
    public TabColors Color { get; set; } = TabColors.Default;

    /// <summary>
    /// Gets or sets an arbitrary CSS width value (e.g., "25%", "200px") applied as an inline style.
    /// When set, the <c>col-{n}</c> class is not applied; width is controlled entirely by this value.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Builds the CSS class string for the placeholder span element.
    /// Includes the base <c>placeholder</c> class, an optional <c>col-{n}</c> column class
    /// (when <see cref="Width"/> is not set), an optional size modifier class,
    /// and an optional background color class.
    /// </summary>
    /// <returns>A string containing all applicable CSS classes separated by spaces.</returns>
    protected override string BuildCssClass()
    {
        var clampedColumns = Math.Clamp(Columns, 1, 12);

        return new CssBuilder("placeholder")
            .AddClass($"col-{clampedColumns}", string.IsNullOrEmpty(Width))
            .AddClass($"placeholder-{Size.GetCssClassName()}", Size != PlaceholderSize.Default)
            .AddClass($"bg-{Color.GetCssClassName()}", Color != TabColors.Default)
            .Build();
    }

    /// <summary>
    /// Builds the inline style string for the placeholder span element.
    /// Adds a <c>width</c> style when <see cref="Width"/> is set.
    /// </summary>
    /// <returns>A string containing all applicable inline styles.</returns>
    protected override string BuildStyleString()
    {
        return new StyleBuilder()
            .AddStyle("width", Width, !string.IsNullOrEmpty(Width))
            .Build();
    }
}
