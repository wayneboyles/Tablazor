using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Tablazor.Attributes;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// Represents a button component that follows the Tabler design system.
/// Supports various colors, sizes, button types, icons, and loading states.
/// </summary>
public partial class TabButton : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the button color.
    /// </summary>
    /// <remarks>
    /// When set to a value other than <see cref="TabColors.Default"/>, the button
    /// will use the corresponding Tabler color class (e.g., btn-primary, btn-danger).
    /// </remarks>
    [Parameter]
    public TabColors Color { get; set; } = TabColors.Default;

    /// <summary>
    /// Gets or sets the button size.
    /// </summary>
    [Parameter]
    public ButtonSize Size { get; set; } = ButtonSize.Default;

    /// <summary>
    /// Gets or sets the button type.
    /// </summary>
    /// <remarks>
    /// Use <see cref="ButtonType.Submit"/> for form submission buttons,
    /// <see cref="ButtonType.Reset"/> for form reset buttons,
    /// <see cref="ButtonType.Link"/> for buttons styled as links, and
    /// <see cref="ButtonType.Button"/> for regular buttons (default).
    /// </remarks>
    [Parameter]
    public ButtonType Type { get; set; } = ButtonType.Button;

    /// <summary>
    /// Gets or sets the URL for link-type buttons.
    /// Only applicable when <see cref="Type"/> is set to <see cref="ButtonType.Link"/>.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the icon to display in the button.
    /// </summary>
    /// <remarks>
    /// Icons should be sourced from <see cref="Tablazor.Icons.TabIcons"/> which provides
    /// both outline and filled versions of icons.
    /// </remarks>
    [Parameter]
    public string? Icon { get; set; }

    /// <summary>
    /// Gets or sets the size of the icon in pixels.
    /// </summary>
    [Parameter]
    public int IconSize { get; set; } = 16;

    /// <summary>
    /// Gets or sets whether the icon should be displayed only (no text).
    /// When true, the button will be styled as an icon-only button with square proportions.
    /// </summary>
    [Parameter]
    public bool IconOnly { get; set; }

    /// <summary>
    /// Gets or sets whether the button should use an outline style.
    /// Outline buttons have a transparent background with a colored border.
    /// </summary>
    [Parameter]
    [NotRequiredIf(nameof(Ghost))]
    public bool Outline { get; set; }

    /// <summary>
    /// Gets or sets whether the button should use a ghost style.
    /// Ghost buttons have no background or border until hovered.
    /// </summary>
    [Parameter]
    [NotRequiredIf(nameof(Outline))]
    public bool Ghost { get; set; }

    /// <summary>
    /// Gets or sets whether the button should be displayed as a square.
    /// </summary>
    [Parameter]
    [NotRequiredIf(nameof(Pill))]
    public bool Square { get; set; }

    /// <summary>
    /// Gets or sets whether the button should be displayed as a pill (rounded corners).
    /// </summary>
    [Parameter]
    [NotRequiredIf(nameof(Square))]
    public bool Pill { get; set; }

    /// <summary>
    /// Gets or sets whether the button should take up the full width of its container.
    /// </summary>
    [Parameter]
    public bool Block { get; set; }

    /// <summary>
    /// Gets or sets whether the button is in a loading state.
    /// When true, displays a spinner and optionally disables the button.
    /// </summary>
    [Parameter]
    public bool IsLoading { get; set; }

    /// <summary>
    /// Gets or sets the content to render inside the button.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Builds the CSS class string for the button.
    /// </summary>
    /// <returns>A string containing the combined CSS classes for the button.</returns>
    protected override string BuildCssClass()
    {
        return new CssBuilder("btn")
            .AddClass(GetColorClass(), Color != TabColors.Default)
            .AddClass($"btn-{Size.GetCssClassName()}", Size != ButtonSize.Default)
            .AddClass("btn-icon", IconOnly)
            .AddClass("btn-square", Square)
            .AddClass("btn-pill", Pill)
            .AddClass("w-100", Block)
            .AddClass("disabled", Disabled && Type == ButtonType.Link)
            .AddClass(CssClass)
            .Build();
    }

    /// <summary>
    /// Gets the appropriate color class based on the button style (outline, ghost, or solid).
    /// </summary>
    /// <returns>The CSS class for the button color.</returns>
    private string GetColorClass()
    {
        return new CssBuilder()
            .AddClass($"btn-ghost-{Color.GetCssClassName()}", when: Ghost)
            .AddClass($"btn-outline-{Color.GetCssClassName()}", when: Outline)
            .AddClass($"btn-{Color.GetCssClassName()}", when: !Ghost && !Outline)
            .Build();
    }

    /// <summary>
    /// Gets the HTML button type attribute value.
    /// </summary>
    /// <returns>The button type string (button, submit, or reset).</returns>
    private string GetButtonType()
    {
        return Type switch
        {
            ButtonType.Submit => "submit",
            ButtonType.Reset => "reset",
            _ => "button"
        };
    }

    /// <summary>
    /// Gets the CSS class for the icon element.
    /// </summary>
    /// <returns>The CSS class string for the icon.</returns>
    private string GetIconCssClass()
    {
        return ChildContent != null && !IconOnly ? "me-1" : string.Empty;
    }

    /// <summary>
    /// Handles the button click event.
    /// </summary>
    /// <param name="args">The mouse event arguments.</param>
    private async Task HandleClickAsync(MouseEventArgs args)
    {
        if (Disabled || IsLoading)
        {
            return;
        }

        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(args);
        }
    }
}
