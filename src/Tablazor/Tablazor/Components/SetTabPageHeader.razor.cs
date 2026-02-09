using Microsoft.AspNetCore.Components;
using Tablazor.Services;

namespace Tablazor.Components;

/// <summary>
/// Helper component used to set the <see cref="TabPageHeader"/> properties
/// </summary>
public partial class SetTabPageHeader : TabBaseComponent
{
    [Inject]
    private TabPageHeaderService HeaderService { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the page header title
    /// </summary>
    [Parameter]
    [EditorRequired]
    public required string Title { get; set; }
    
    /// <summary>
    /// Gets or sets the page header subtitle
    /// </summary>
    [Parameter]
    public string? Subtitle { get; set; }
    
    /// <summary>
    /// Gets or sets the content to render at the end of the page
    /// header such as page actions.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    
    /// <summary>
    /// Whether to render breadcrumbs in the header
    /// </summary>
    [Parameter]
    public bool ShowBreadcrumbs { get; set; }

    /// <summary>
    /// Method invoked when the component is ready to start, having received its
    /// initial parameters from its parent in the render tree.
    /// </summary>
    protected override void OnInitialized()
    {
        UpdatePageHeader();
    }

    /// <summary>
    /// Method invoked when the component has received parameters from its parent in
    /// the render tree, and the incoming values have been assigned to properties.
    /// </summary>
    protected override void OnParametersSet()
    {
        UpdatePageHeader();
    }

    private void UpdatePageHeader() => HeaderService.SetPageHeader(Title, Subtitle, ChildContent, ShowBreadcrumbs);
}