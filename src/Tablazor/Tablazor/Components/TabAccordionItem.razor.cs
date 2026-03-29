using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// A collapsible panel used inside a <see cref="TabAccordion"/>.
/// </summary>
public partial class TabAccordionItem : TabBaseComponent
{
    private bool _isOpen;

    /// <summary>
    /// Gets or sets the parent <see cref="TabAccordion"/> this item belongs to.
    /// </summary>
    [CascadingParameter]
    private TabAccordion? ParentAccordion { get; set; }

    /// <summary>
    /// Gets or sets the header button text.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets an optional identifier used to distinguish this item in accordion-level events
    /// (<see cref="TabAccordion.OnItemOpened"/>, <see cref="TabAccordion.OnItemClosed"/>).
    /// </summary>
    [Parameter]
    public string? ItemId { get; set; }

    /// <summary>
    /// Gets or sets a custom header template. When set, takes precedence over <see cref="Title"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? HeaderContent { get; set; }

    /// <summary>
    /// Gets or sets the body content of this panel.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this item is open on first render.
    /// </summary>
    [Parameter]
    public bool IsOpen { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when this item is opened.
    /// </summary>
    [Parameter]
    public EventCallback OnOpened { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when this item is closed.
    /// </summary>
    [Parameter]
    public EventCallback OnClosed { get; set; }

    /// <summary>
    /// Gets whether this item is currently open.
    /// </summary>
    public bool IsCurrentlyOpen => ParentAccordion?.IsItemOpen(this) ?? _isOpen;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _isOpen = IsOpen;
        ParentAccordion?.RegisterItem(this);
    }

    private async Task HandleToggleAsync()
    {
        if (Disabled) return;

        var willOpen = !IsCurrentlyOpen;

        if (ParentAccordion is not null)
        {
            await ParentAccordion.ToggleItemAsync(this);
        }
        else
        {
            _isOpen = !_isOpen;
            StateHasChanged();
        }

        if (willOpen && OnOpened.HasDelegate)
        {
            await OnOpened.InvokeAsync();
        }
        else if (!willOpen && OnClosed.HasDelegate)
        {
            await OnClosed.InvokeAsync();
        }
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        ParentAccordion?.UnregisterItem(this);
        base.Dispose();
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("accordion-item")
            .AddClass(CssClass)
            .Build();
    }

    private string GetButtonCssClass()
    {
        return new CssBuilder("accordion-button")
            .AddClass("collapsed", !IsCurrentlyOpen)
            .Build();
    }

    private string GetCollapseCssClass()
    {
        return new CssBuilder("accordion-collapse collapse")
            .AddClass("show", IsCurrentlyOpen)
            .Build();
    }
}
