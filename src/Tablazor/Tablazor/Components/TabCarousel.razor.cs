using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// A carousel component for cycling through slides with support for images, captions, and various indicator styles.
/// Based on the Tabler design system.
/// </summary>
/// <remarks>
/// Use <see cref="TabCarouselSlide"/> components as children to define individual slides.
/// Supports automatic cycling, keyboard navigation, and touch/swipe gestures.
/// </remarks>
/// <example>
/// <code>
/// &lt;TabCarousel&gt;
///     &lt;TabCarouselSlide ImageUrl="image1.jpg" CaptionTitle="First Slide" /&gt;
///     &lt;TabCarouselSlide ImageUrl="image2.jpg" CaptionTitle="Second Slide" /&gt;
/// &lt;/TabCarousel&gt;
/// </code>
/// </example>
public partial class TabCarousel : TabBaseComponent, IAsyncDisposable
{
    private readonly List<TabCarouselSlide> _slides = new();
    private int _activeIndex;
    private bool _isTransitioning;
    private CancellationTokenSource? _autoPlayCts;
    private string _carouselId = string.Empty;
    private bool _isPaused;

    /// <summary>
    /// Gets or sets the child content containing <see cref="TabCarouselSlide"/> components.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the index of the currently active slide.
    /// </summary>
    /// <value>The default value is 0.</value>
    [Parameter]
    public int ActiveIndex { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the active slide changes.
    /// </summary>
    [Parameter]
    public EventCallback<int> ActiveIndexChanged { get; set; }

    /// <summary>
    /// Gets or sets the transition effect between slides.
    /// </summary>
    /// <value>The default value is <see cref="CarouselTransition.Slide"/>.</value>
    [Parameter]
    public CarouselTransition Transition { get; set; } = CarouselTransition.Slide;

    /// <summary>
    /// Gets or sets a value indicating whether to show navigation indicators.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    [Parameter]
    public bool ShowIndicators { get; set; } = true;

    /// <summary>
    /// Gets or sets the style of the navigation indicators.
    /// </summary>
    /// <value>The default value is <see cref="CarouselIndicatorStyle.Default"/>.</value>
    [Parameter]
    public CarouselIndicatorStyle IndicatorStyle { get; set; } = CarouselIndicatorStyle.Default;

    /// <summary>
    /// Gets or sets a value indicating whether to show indicators vertically on the right side.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    [Parameter]
    public bool VerticalIndicators { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show previous/next navigation controls.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    [Parameter]
    public bool ShowControls { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the carousel should automatically cycle through slides.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    [Parameter]
    public bool AutoPlay { get; set; }

    /// <summary>
    /// Gets or sets the interval in milliseconds between automatic slide transitions.
    /// </summary>
    /// <value>The default value is 5000 (5 seconds).</value>
    [Parameter]
    public int Interval { get; set; } = 5000;

    /// <summary>
    /// Gets or sets a value indicating whether to pause auto-play on hover.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    [Parameter]
    public bool PauseOnHover { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to enable keyboard navigation.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    [Parameter]
    public bool EnableKeyboard { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the carousel should wrap around from the last slide to the first.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    [Parameter]
    public bool Wrap { get; set; } = true;

    /// <summary>
    /// Gets or sets the duration of slide transitions in milliseconds.
    /// </summary>
    /// <value>The default value is 600.</value>
    [Parameter]
    public int TransitionDuration { get; set; } = 600;

    /// <summary>
    /// Gets or sets the callback invoked when a slide transition starts.
    /// </summary>
    [Parameter]
    public EventCallback<int> OnSlideStart { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when a slide transition completes.
    /// </summary>
    [Parameter]
    public EventCallback<int> OnSlideEnd { get; set; }

    /// <summary>
    /// Gets the number of slides in the carousel.
    /// </summary>
    public int SlideCount => _slides.Count;

    /// <summary>
    /// Gets the current active slide index.
    /// </summary>
    public int CurrentIndex => _activeIndex;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _carouselId = GetId();
        _activeIndex = ActiveIndex;
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (ActiveIndex != _activeIndex && !_isTransitioning)
        {
            _ = GoToSlideAsync(ActiveIndex);
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            UpdateSlideStates();

            if (AutoPlay)
            {
                StartAutoPlay();
            }
        }
    }

    /// <summary>
    /// Adds a slide to the carousel.
    /// </summary>
    internal void AddSlide(TabCarouselSlide slide)
    {
        slide.Index = _slides.Count;
        _slides.Add(slide);

        if (_slides.Count == 1 || slide.Index == _activeIndex)
        {
            slide.IsActive = slide.Index == _activeIndex;
        }

        StateHasChanged();
    }

    /// <summary>
    /// Removes a slide from the carousel.
    /// </summary>
    internal void RemoveSlide(TabCarouselSlide slide)
    {
        _slides.Remove(slide);

        // Reindex remaining slides
        for (var i = 0; i < _slides.Count; i++)
        {
            _slides[i].Index = i;
        }

        if (_activeIndex >= _slides.Count && _slides.Count > 0)
        {
            _activeIndex = _slides.Count - 1;
        }

        StateHasChanged();
    }

    /// <summary>
    /// Navigates to the next slide.
    /// </summary>
    public async Task NextAsync()
    {
        if (_isTransitioning || _slides.Count <= 1)
        {
            return;
        }

        var nextIndex = _activeIndex + 1;

        if (nextIndex >= _slides.Count)
        {
            if (!Wrap)
            {
                return;
            }

            nextIndex = 0;
        }

        await TransitionToSlideAsync(nextIndex, true);
    }

    /// <summary>
    /// Navigates to the previous slide.
    /// </summary>
    public async Task PreviousAsync()
    {
        if (_isTransitioning || _slides.Count <= 1)
        {
            return;
        }

        var prevIndex = _activeIndex - 1;

        if (prevIndex < 0)
        {
            if (!Wrap)
            {
                return;
            }

            prevIndex = _slides.Count - 1;
        }

        await TransitionToSlideAsync(prevIndex, false);
    }

    /// <summary>
    /// Navigates to a specific slide by index.
    /// </summary>
    /// <param name="index">The index of the slide to navigate to.</param>
    public async Task GoToSlideAsync(int index)
    {
        if (_isTransitioning || index < 0 || index >= _slides.Count || index == _activeIndex)
        {
            return;
        }

        var isNext = index > _activeIndex;
        await TransitionToSlideAsync(index, isNext);
    }

    /// <summary>
    /// Pauses automatic slide cycling.
    /// </summary>
    public void Pause()
    {
        _isPaused = true;
    }

    /// <summary>
    /// Resumes automatic slide cycling.
    /// </summary>
    public void Resume()
    {
        _isPaused = false;
    }

    private async Task TransitionToSlideAsync(int newIndex, bool isNext)
    {
        if (_slides.Count == 0)
        {
            return;
        }

        _isTransitioning = true;

        var currentSlide = _slides.ElementAtOrDefault(_activeIndex);
        var nextSlide = _slides.ElementAtOrDefault(newIndex);

        if (currentSlide is null || nextSlide is null)
        {
            _isTransitioning = false;
            return;
        }

        if (OnSlideStart.HasDelegate)
        {
            await OnSlideStart.InvokeAsync(newIndex);
        }

        // Set up transition classes
        if (isNext)
        {
            nextSlide.IsNext = true;
        }
        else
        {
            nextSlide.IsPrev = true;
        }

        await InvokeAsync(StateHasChanged);

        // Small delay to ensure the browser has applied the positioning class
        await Task.Delay(20);

        // Apply the transform classes
        if (isNext)
        {
            currentSlide.IsStart = true;
            nextSlide.IsStart = true;
        }
        else
        {
            currentSlide.IsEnd = true;
            nextSlide.IsEnd = true;
        }

        await InvokeAsync(StateHasChanged);

        // Wait for transition to complete
        await Task.Delay(TransitionDuration);

        // Clean up and finalize
        currentSlide.IsActive = false;
        currentSlide.IsStart = false;
        currentSlide.IsEnd = false;

        nextSlide.IsActive = true;
        nextSlide.IsNext = false;
        nextSlide.IsPrev = false;
        nextSlide.IsStart = false;
        nextSlide.IsEnd = false;

        _activeIndex = newIndex;
        _isTransitioning = false;

        if (ActiveIndexChanged.HasDelegate)
        {
            await ActiveIndexChanged.InvokeAsync(_activeIndex);
        }

        if (OnSlideEnd.HasDelegate)
        {
            await OnSlideEnd.InvokeAsync(_activeIndex);
        }

        await InvokeAsync(StateHasChanged);
    }

    private void UpdateSlideStates()
    {
        for (var i = 0; i < _slides.Count; i++)
        {
            _slides[i].IsActive = i == _activeIndex;
            _slides[i].IsNext = false;
            _slides[i].IsPrev = false;
            _slides[i].IsStart = false;
            _slides[i].IsEnd = false;
        }
    }

    private void StartAutoPlay()
    {
        _autoPlayCts?.Cancel();
        _autoPlayCts = new CancellationTokenSource();

        _ = AutoPlayLoopAsync(_autoPlayCts.Token);
    }

    private async Task AutoPlayLoopAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                // Get the current slide's interval or use default
                var currentSlide = _slides.ElementAtOrDefault(_activeIndex);
                var interval = currentSlide?.Interval ?? Interval;

                await Task.Delay(interval, cancellationToken);

                if (!_isPaused && !_isTransitioning)
                {
                    await InvokeAsync(NextAsync);
                }
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }
    }

    private async Task HandleIndicatorClickAsync(int index)
    {
        await GoToSlideAsync(index);
    }

    private async Task HandlePreviousClickAsync()
    {
        await PreviousAsync();
    }

    private async Task HandleNextClickAsync()
    {
        await NextAsync();
    }

    private void HandleMouseEnter()
    {
        if (PauseOnHover && AutoPlay)
        {
            Pause();
        }
    }

    private void HandleMouseLeave()
    {
        if (PauseOnHover && AutoPlay)
        {
            Resume();
        }
    }

    private async Task HandleKeyDownAsync(KeyboardEventArgs e)
    {
        if (!EnableKeyboard)
        {
            return;
        }

        switch (e.Key)
        {
            case "ArrowLeft":
                await PreviousAsync();
                break;
            case "ArrowRight":
                await NextAsync();
                break;
        }
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("carousel slide")
            .AddClass(Transition.GetCssClassName(), Transition != CarouselTransition.Slide)
            .AddClass(CssClass)
            .Build();
    }

    private string BuildIndicatorsCssClass()
    {
        return new CssBuilder("carousel-indicators")
            .AddClass(IndicatorStyle.GetCssClassName(), IndicatorStyle != CarouselIndicatorStyle.Default)
            .AddClass("carousel-indicators-vertical", VerticalIndicators)
            .Build();
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        _autoPlayCts?.Cancel();
        _autoPlayCts?.Dispose();

        await base.DisposeAsync();
    }
}
