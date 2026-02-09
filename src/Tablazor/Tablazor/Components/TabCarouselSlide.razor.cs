using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// Represents a single slide within a <see cref="TabCarousel"/> component.
/// </summary>
public partial class TabCarouselSlide : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the content to render inside the slide.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the image URL for the slide background.
    /// </summary>
    [Parameter]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the alt text for the slide image.
    /// </summary>
    [Parameter]
    public string? ImageAlt { get; set; }

    /// <summary>
    /// Gets or sets the caption title for the slide.
    /// </summary>
    [Parameter]
    public string? CaptionTitle { get; set; }

    /// <summary>
    /// Gets or sets the caption description for the slide.
    /// </summary>
    [Parameter]
    public string? CaptionText { get; set; }

    /// <summary>
    /// Gets or sets custom caption content. Takes precedence over CaptionTitle and CaptionText.
    /// </summary>
    [Parameter]
    public RenderFragment? CaptionContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show a dark background overlay behind the caption.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    [Parameter]
    public bool ShowCaptionBackground { get; set; }

    /// <summary>
    /// Gets or sets the thumbnail image URL for thumbnail-style indicators.
    /// If not set, the main ImageUrl will be used.
    /// </summary>
    [Parameter]
    public string? ThumbnailUrl { get; set; }

    /// <summary>
    /// Gets or sets the interval in milliseconds for this specific slide.
    /// If set, overrides the carousel's default interval for this slide.
    /// </summary>
    [Parameter]
    public int? Interval { get; set; }

    /// <summary>
    /// Gets or sets the parent carousel component.
    /// </summary>
    [CascadingParameter]
    internal TabCarousel? ParentCarousel { get; set; }

    /// <summary>
    /// Gets the index of this slide within the carousel.
    /// </summary>
    internal int Index { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this slide is currently active.
    /// </summary>
    internal bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this slide is transitioning in from the next position.
    /// </summary>
    internal bool IsNext { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this slide is transitioning in from the previous position.
    /// </summary>
    internal bool IsPrev { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to apply the start transform class.
    /// </summary>
    internal bool IsStart { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to apply the end transform class.
    /// </summary>
    internal bool IsEnd { get; set; }

    /// <summary>
    /// Gets the effective thumbnail URL.
    /// </summary>
    internal string? EffectiveThumbnailUrl => ThumbnailUrl ?? ImageUrl;

    /// <summary>
    /// Gets a value indicating whether this slide has a caption.
    /// </summary>
    internal bool HasCaption => CaptionContent is not null ||
                                !string.IsNullOrEmpty(CaptionTitle) ||
                                !string.IsNullOrEmpty(CaptionText);

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        ParentCarousel?.AddSlide(this);
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("carousel-item")
            .AddClass("active", IsActive && !IsNext && !IsPrev)
            .AddClass("carousel-item-next", IsNext)
            .AddClass("carousel-item-prev", IsPrev)
            .AddClass("carousel-item-start", IsStart)
            .AddClass("carousel-item-end", IsEnd)
            .AddClass(CssClass)
            .Build();
    }

    private string BuildCaptionCssClass()
    {
        return new CssBuilder("carousel-caption")
            .AddClass("d-none d-md-block")
            .Build();
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        ParentCarousel?.RemoveSlide(this);
        base.Dispose();
    }
}
