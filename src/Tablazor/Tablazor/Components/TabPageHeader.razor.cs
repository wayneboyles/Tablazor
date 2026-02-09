using Microsoft.AspNetCore.Components;
using Tablazor.Services;

namespace Tablazor.Components;

/// <summary>
/// Page headers provide the ability to display a title, optional
/// subtitle as well as page level actions
/// </summary>
public partial class TabPageHeader : TabBaseComponent
{
    [Inject]
    internal TabPageHeaderService? HeaderService { get; set; }
    
    /// <summary>
    /// Gets or sets the content for the page actions
    /// </summary>
    [Parameter]
    public RenderFragment? Actions { get; set; }

    /// <summary>
    /// Method invoked when the component is ready to start, having received its
    /// initial parameters from its parent in the render tree.
    /// </summary>
    protected override void OnInitialized()
    {
        if (HeaderService != null)
        {
            HeaderService.OnChange += StateHasChanged;
        }
    }

    /// <summary>
    /// Builds a CSS class attribute value by combining the specified base class with any additional CSS classes
    /// defined in the current instance.
    /// </summary>
    /// <returns>A string containing the combined CSS classes, separated by spaces. If no additional CSS classes are defined,
    /// returns the base class.</returns>
    protected override string BuildCssClass()
    {
        return new CssBuilder("page-header")
            .Build();
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public override void Dispose()
    {
        if (HeaderService != null)
        {
            HeaderService.OnChange -= StateHasChanged;
        }

        base.Dispose();
    }
}