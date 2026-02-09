namespace Tablazor.Components;

/// <summary>
/// Represents a divider line within a <see cref="TabDropdownMenu"/>.
/// Used to visually separate groups of dropdown items.
/// </summary>
public partial class TabDropdownDivider : TabBaseComponent
{
    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("dropdown-divider")
            .AddClass(CssClass)
            .Build();
    }
}
