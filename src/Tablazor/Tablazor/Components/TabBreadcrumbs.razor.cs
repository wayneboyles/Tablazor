using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// A breadcrumb navigation component that displays the current location within a navigational hierarchy.
/// Supports both dynamic generation based on the current URL and static item definition.
/// </summary>
public partial class TabBreadcrumbs : TabBaseComponent
{
    private List<BreadcrumbItem> _generatedItems = new();
    private HashSet<string> _excludedSegmentsSet = new(StringComparer.OrdinalIgnoreCase);
    
    /// <summary>
    /// Gets or sets the navigation manager used for dynamic breadcrumb generation.
    /// </summary>
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the separator style for the breadcrumb items.
    /// </summary>
    [Parameter]
    public BreadcrumbSeparator Separator { get; set; } = BreadcrumbSeparator.Default;

    /// <summary>
    /// Gets or sets a static list of breadcrumb items to display.
    /// When set, automatic URL-based generation is disabled.
    /// </summary>
    [Parameter]
    public IEnumerable<BreadcrumbItem>? Items { get; set; }

    /// <summary>
    /// Gets or sets the child content (manual TabBreadcrumbItem components).
    /// When set, automatic URL-based generation is disabled.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets whether to automatically generate breadcrumb items based on the current URL.
    /// When true, breadcrumbs are built from URL path segments.
    /// Default is true. Set to false when using Items or ChildContent.
    /// </summary>
    [Parameter]
    public bool AutoGenerate { get; set; } = true;

    /// <summary>
    /// Gets or sets the text to display for the home/root breadcrumb item.
    /// Only used when AutoGenerate is true.
    /// </summary>
    [Parameter]
    public string HomeText { get; set; } = "Home";

    /// <summary>
    /// Gets or sets the URL for the home/root breadcrumb item.
    /// Only used when AutoGenerate is true.
    /// </summary>
    [Parameter]
    public string HomeHref { get; set; } = "/";

    /// <summary>
    /// Gets or sets the icon for the home breadcrumb item.
    /// Only used when AutoGenerate is true.
    /// </summary>
    [Parameter]
    public string? HomeIcon { get; set; }

    /// <summary>
    /// Gets or sets a custom function to transform URL segments into display text.
    /// Receives the raw URL segment and returns the display text.
    /// If not set, segments are converted using title case with hyphens replaced by spaces.
    /// </summary>
    [Parameter]
    public Func<string, string>? SegmentTransformer { get; set; }

    /// <summary>
    /// Gets or sets URL segments to exclude from the breadcrumb trail.
    /// Useful for filtering out route parameters or technical segments.
    /// </summary>
    [Parameter]
    public IEnumerable<string>? ExcludedSegments { get; set; }

    /// <summary>
    /// Gets or sets whether to show the home item when auto-generating.
    /// Default is true.
    /// </summary>
    [Parameter]
    public bool ShowHome { get; set; } = true;

    /// <summary>
    /// Gets or sets a callback invoked when a breadcrumb item is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<BreadcrumbItem> OnItemClick { get; set; }

    /// <summary>
    /// Gets the breadcrumb items to render.
    /// </summary>
    protected IEnumerable<BreadcrumbItem> BreadcrumbItems =>
        Items ?? (AutoGenerate && ChildContent == null ? _generatedItems : Enumerable.Empty<BreadcrumbItem>());

    /// <summary>
    /// Gets whether to render items from the Items collection or auto-generated list.
    /// </summary>
    protected bool HasItems => Items != null || (AutoGenerate && ChildContent == null && _generatedItems.Count > 0);

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        NavigationManager.LocationChanged += OnLocationChanged;
        UpdateExcludedSegments();
        GenerateBreadcrumbs();
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        UpdateExcludedSegments();

        if (AutoGenerate && Items == null && ChildContent == null)
        {
            GenerateBreadcrumbs();
        }
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("breadcrumb")
            .AddClass(Separator.GetCssClassName(), Separator != BreadcrumbSeparator.Default)
            .AddClass(CssClass)
            .Build();
    }

    private void UpdateExcludedSegments()
    {
        _excludedSegmentsSet = ExcludedSegments != null
            ? new HashSet<string>(ExcludedSegments, StringComparer.OrdinalIgnoreCase)
            : new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        if (AutoGenerate && Items == null && ChildContent == null)
        {
            GenerateBreadcrumbs();
            StateHasChanged();
        }
    }

    private void GenerateBreadcrumbs()
    {
        _generatedItems.Clear();

        var uri = new Uri(NavigationManager.Uri);
        var segments = uri.AbsolutePath
            .Split('/', StringSplitOptions.RemoveEmptyEntries)
            .Where(s => !_excludedSegmentsSet.Contains(s))
            .ToList();

        // Add home item
        if (ShowHome)
        {
            _generatedItems.Add(new BreadcrumbItem
            {
                Text = HomeText,
                Href = HomeHref,
                Icon = HomeIcon,
                IsActive = segments.Count == 0
            });
        }

        // Build path progressively and add each segment
        var pathBuilder = string.Empty;
        for (var i = 0; i < segments.Count; i++)
        {
            var segment = segments[i];
            pathBuilder += "/" + segment;

            var isLast = i == segments.Count - 1;
            var displayText = TransformSegment(segment);

            _generatedItems.Add(new BreadcrumbItem
            {
                Text = displayText,
                Href = isLast ? null : pathBuilder,
                IsActive = isLast
            });
        }
    }

    private string TransformSegment(string segment)
    {
        if (SegmentTransformer != null)
        {
            return SegmentTransformer(segment);
        }

        // Default transformation: replace hyphens/underscores with spaces and use title case
        var text = segment.Replace('-', ' ').Replace('_', ' ');
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLowerInvariant());
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
        base.Dispose();
    }
}
