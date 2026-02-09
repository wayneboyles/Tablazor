using Microsoft.AspNetCore.Components;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// Represents the menu container within a <see cref="TabDropdown"/> component.
/// Contains <see cref="TabDropdownItem"/>, <see cref="TabDropdownDivider"/>,
/// and <see cref="TabDropdownHeader"/> components.
/// </summary>
public partial class TabDropdownMenu : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the child content of the dropdown menu.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the alignment of the dropdown menu relative to its toggle element.
    /// </summary>
    /// <value>The default value is <see cref="DropdownAlignment.Start"/>.</value>
    [Parameter]
    public DropdownAlignment Alignment { get; set; } = DropdownAlignment.Start;

    /// <summary>
    /// Gets or sets the parent dropdown component.
    /// </summary>
    [CascadingParameter]
    internal TabDropdown? ParentDropdown { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        ParentDropdown?.RegisterMenu(this);
    }

    /// <summary>
    /// Called by the parent <see cref="TabDropdown"/> when its open state changes.
    /// </summary>
    internal void NotifyStateChanged()
    {
        InvokeAsync(StateHasChanged);
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("dropdown-menu")
            .AddClass("show", ParentDropdown?.IsMenuOpen == true)
            .AddClass(Alignment.GetCssClassName(), Alignment != DropdownAlignment.Start)
            .AddClass(CssClass)
            .Build();
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        ParentDropdown?.UnregisterMenu(this);
        base.Dispose();
    }
}
