using Microsoft.AspNetCore.Components;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// A card component that provides a flexible container for displaying content.
/// Cards support headers, bodies, footers, status borders, padding variations, and more.
/// </summary>
/// <remarks>
/// Use <see cref="TabCardHeader"/>, <see cref="TabCardBody"/>, and <see cref="TabCardFooter"/>
/// as child components to structure the card content. The <see cref="TabCardTitle"/> and
/// <see cref="TabCardSubtitle"/> components can be used within the header for consistent styling.
/// </remarks>
public partial class TabCard : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the content to render inside the card.
    /// </summary>
    /// <remarks>
    /// Typically contains <see cref="TabCardHeader"/>, <see cref="TabCardBody"/>,
    /// and/or <see cref="TabCardFooter"/> components for structured layouts.
    /// </remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the padding size for the card.
    /// </summary>
    /// <value>The default value is <see cref="CardPadding.Default"/>.</value>
    [Parameter]
    public CardPadding Padding { get; set; } = CardPadding.Default;

    /// <summary>
    /// Gets or sets the position of the status border.
    /// </summary>
    /// <value>The default value is <see cref="CardStatusPosition.None"/>.</value>
    /// <remarks>
    /// Use in combination with <see cref="StatusColor"/> to display a colored
    /// status indicator on the card.
    /// </remarks>
    [Parameter]
    public CardStatusPosition StatusPosition { get; set; } = CardStatusPosition.None;

    /// <summary>
    /// Gets or sets the color of the status border.
    /// </summary>
    /// <value>The default value is <see cref="TabColors.Default"/>.</value>
    /// <remarks>
    /// Only applies when <see cref="StatusPosition"/> is set to a value other than
    /// <see cref="CardStatusPosition.None"/>.
    /// </remarks>
    [Parameter]
    public TabColors StatusColor { get; set; } = TabColors.Default;

    /// <summary>
    /// Gets or sets a value indicating whether the card should have a stacked appearance.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    /// <remarks>
    /// Stacked cards have a visual effect that makes them appear as if multiple cards
    /// are stacked behind them.
    /// </remarks>
    [Parameter]
    public bool Stacked { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the card should be rendered without a border.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    [Parameter]
    public bool Borderless { get; set; }

    /// <summary>
    /// Builds a CSS class attribute value by combining the specified base class with any additional CSS classes
    /// defined in the current instance.
    /// </summary>
    /// <returns>A string containing the combined CSS classes, separated by spaces. If no additional CSS classes are defined,
    /// returns the base class.</returns>
    protected override string BuildCssClass()
    {
        return new CssBuilder("card")
            .AddClass($"card-{Padding.GetCssClassName()}", Padding != CardPadding.Default)
            .AddClass($"card-status-{StatusPosition.GetCssClassName()}", StatusPosition != CardStatusPosition.None)
            .AddClass($"bg-{StatusColor.GetCssClassName()}", StatusPosition != CardStatusPosition.None && StatusColor != TabColors.Default)
            .AddClass("card-stacked", Stacked)
            .AddClass("card-borderless", Borderless)
            .Build();
    }
}
