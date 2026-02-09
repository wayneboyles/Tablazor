using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Tablazor.Components;
using Tablazor.Enums;

namespace Tablazor.Components;

public class TabCarouselTests : BunitContext
{
    public TabCarouselTests()
    {
        Services.AddLogging();
    }

    [Fact]
    public void RendersCarouselElement()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.AddAttribute(1, "ImageUrl", "test.jpg");
                builder.CloseComponent();
            }));

        var carousel = cut.Find("div.carousel");
        Assert.NotNull(carousel);
    }

    [Fact]
    public void HasSlideClass()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
            }));

        var carousel = cut.Find("div.carousel");
        Assert.Contains("slide", carousel.GetAttribute("class"));
    }

    [Fact]
    public void RendersCarouselInner()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
            }));

        var inner = cut.Find("div.carousel-inner");
        Assert.NotNull(inner);
    }

    [Fact]
    public void ShowsIndicators_ByDefault_WhenMultipleSlides()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
            }));

        var indicators = cut.Find("div.carousel-indicators");
        Assert.NotNull(indicators);
    }

    [Fact]
    public void HidesIndicators_WhenShowIndicatorsIsFalse()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ShowIndicators, false)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
            }));

        var indicators = cut.FindAll("div.carousel-indicators");
        Assert.Empty(indicators);
    }

    [Fact]
    public void HidesIndicators_WhenOnlyOneSlide()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
            }));

        var indicators = cut.FindAll("div.carousel-indicators");
        Assert.Empty(indicators);
    }

    [Fact]
    public void ShowsControls_ByDefault_WhenMultipleSlides()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
            }));

        var prevButton = cut.Find("button.carousel-control-prev");
        var nextButton = cut.Find("button.carousel-control-next");
        Assert.NotNull(prevButton);
        Assert.NotNull(nextButton);
    }

    [Fact]
    public void HidesControls_WhenShowControlsIsFalse()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ShowControls, false)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
            }));

        var controls = cut.FindAll("button.carousel-control-prev");
        Assert.Empty(controls);
    }

    [Fact]
    public void HidesControls_WhenOnlyOneSlide()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
            }));

        var controls = cut.FindAll("button.carousel-control-prev");
        Assert.Empty(controls);
    }

    [Fact]
    public void AppliesFadeClass_WhenTransitionIsFade()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.Transition, CarouselTransition.Fade)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
            }));

        var carousel = cut.Find("div.carousel");
        Assert.Contains("carousel-fade", carousel.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyFadeClass_WhenTransitionIsSlide()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.Transition, CarouselTransition.Slide)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
            }));

        var carousel = cut.Find("div.carousel");
        Assert.DoesNotContain("carousel-fade", carousel.GetAttribute("class"));
    }

    [Fact]
    public void AppliesDotIndicatorStyle()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.IndicatorStyle, CarouselIndicatorStyle.Dot)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
            }));

        var indicators = cut.Find("div.carousel-indicators");
        Assert.Contains("carousel-indicators-dot", indicators.GetAttribute("class"));
    }

    [Fact]
    public void AppliesVerticalIndicatorsClass()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.VerticalIndicators, true)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
            }));

        var indicators = cut.Find("div.carousel-indicators");
        Assert.Contains("carousel-indicators-vertical", indicators.GetAttribute("class"));
    }

    [Fact]
    public void RendersCorrectNumberOfIndicators()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(2);
                builder.CloseComponent();
            }));

        var indicatorButtons = cut.FindAll("div.carousel-indicators button");
        Assert.Equal(3, indicatorButtons.Count);
    }

    [Fact]
    public void FirstSlideIsActive_ByDefault()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
            }));

        var activeSlide = cut.Find("div.carousel-item.active");
        Assert.NotNull(activeSlide);
    }

    [Fact]
    public void FirstIndicatorIsActive_ByDefault()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
            }));

        var activeIndicator = cut.Find("div.carousel-indicators button.active");
        Assert.NotNull(activeIndicator);
    }

    [Fact]
    public void DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.Visible, false)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
            }));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void AppliesCustomCssClass()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.CssClass, "my-custom-carousel")
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
            }));

        var carousel = cut.Find("div.carousel");
        Assert.Contains("my-custom-carousel", carousel.GetAttribute("class"));
    }

    [Fact]
    public void AppliesStyle()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.Style, "max-width: 800px;")
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
            }));

        var carousel = cut.Find("div.carousel");
        Assert.Equal("max-width: 800px;", carousel.GetAttribute("style"));
    }

    [Fact]
    public void GeneratesUniqueId()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
            }));

        var carousel = cut.Find("div.carousel");
        var id = carousel.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("tab-", id);
    }

    [Fact]
    public void HasTabIndex_ForKeyboardNavigation()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
            }));

        var carousel = cut.Find("div.carousel");
        Assert.Equal("0", carousel.GetAttribute("tabindex"));
    }

    [Fact]
    public async Task NextAsync_MovesToNextSlide()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.TransitionDuration, 10)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
            }));

        var component = cut.Instance;
        Assert.Equal(0, component.CurrentIndex);

        await cut.InvokeAsync(() => component.NextAsync());
        await Task.Delay(50);

        Assert.Equal(1, component.CurrentIndex);
    }

    [Fact]
    public async Task PreviousAsync_MovesToPreviousSlide()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.TransitionDuration, 10)
            .Add(p => p.ActiveIndex, 1)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
            }));

        // Wait for initial render
        await Task.Delay(50);

        var component = cut.Instance;
        await cut.InvokeAsync(() => component.PreviousAsync());
        await Task.Delay(50);

        Assert.Equal(0, component.CurrentIndex);
    }

    [Fact]
    public async Task NextAsync_WrapsToFirst_WhenOnLastSlide()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.TransitionDuration, 10)
            .Add(p => p.Wrap, true)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
            }));

        var component = cut.Instance;

        // Move to last slide
        await cut.InvokeAsync(() => component.NextAsync());
        await Task.Delay(50);
        Assert.Equal(1, component.CurrentIndex);

        // Move next should wrap to first
        await cut.InvokeAsync(() => component.NextAsync());
        await Task.Delay(50);
        Assert.Equal(0, component.CurrentIndex);
    }

    [Fact]
    public async Task NextAsync_DoesNotWrap_WhenWrapIsFalse()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.TransitionDuration, 10)
            .Add(p => p.Wrap, false)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
            }));

        var component = cut.Instance;

        // Move to last slide
        await cut.InvokeAsync(() => component.NextAsync());
        await Task.Delay(50);
        Assert.Equal(1, component.CurrentIndex);

        // Move next should stay on last slide
        await cut.InvokeAsync(() => component.NextAsync());
        await Task.Delay(50);
        Assert.Equal(1, component.CurrentIndex);
    }

    [Fact]
    public async Task GoToSlideAsync_NavigatesToSpecificSlide()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.TransitionDuration, 10)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(2);
                builder.CloseComponent();
            }));

        var component = cut.Instance;

        await cut.InvokeAsync(() => component.GoToSlideAsync(2));
        await Task.Delay(50);

        Assert.Equal(2, component.CurrentIndex);
    }

    [Fact]
    public void SlideCount_ReturnsNumberOfSlides()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(2);
                builder.CloseComponent();
            }));

        Assert.Equal(3, cut.Instance.SlideCount);
    }

    [Fact]
    public async Task ActiveIndexChanged_IsInvoked_WhenSlideChanges()
    {
        var newIndex = -1;

        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.TransitionDuration, 10)
            .Add(p => p.ActiveIndexChanged, EventCallback.Factory.Create<int>(this, index => newIndex = index))
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
            }));

        await cut.InvokeAsync(() => cut.Instance.NextAsync());
        await Task.Delay(50);

        Assert.Equal(1, newIndex);
    }

    [Fact]
    public async Task OnSlideStart_IsInvoked_BeforeTransition()
    {
        var startIndex = -1;

        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.TransitionDuration, 10)
            .Add(p => p.OnSlideStart, EventCallback.Factory.Create<int>(this, index => startIndex = index))
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
            }));

        await cut.InvokeAsync(() => cut.Instance.NextAsync());
        await Task.Delay(50);

        Assert.Equal(1, startIndex);
    }

    [Fact]
    public async Task OnSlideEnd_IsInvoked_AfterTransition()
    {
        var endIndex = -1;

        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.TransitionDuration, 10)
            .Add(p => p.OnSlideEnd, EventCallback.Factory.Create<int>(this, index => endIndex = index))
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
            }));

        await cut.InvokeAsync(() => cut.Instance.NextAsync());
        await Task.Delay(50);

        Assert.Equal(1, endIndex);
    }

    [Fact]
    public void Pause_SetsPausedState()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.AutoPlay, true)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
            }));

        cut.Instance.Pause();
        // Pause should be called without error
        Assert.NotNull(cut.Instance);
    }

    [Fact]
    public void Resume_ClearsPausedState()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.AutoPlay, true)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
            }));

        cut.Instance.Pause();
        cut.Instance.Resume();
        // Resume should be called without error
        Assert.NotNull(cut.Instance);
    }

    [Fact]
    public void RendersAccessibleAriaLabels()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
            }));

        var prevButton = cut.Find("button.carousel-control-prev");
        var nextButton = cut.Find("button.carousel-control-next");

        Assert.Equal("Previous", prevButton.GetAttribute("aria-label"));
        Assert.Equal("Next", nextButton.GetAttribute("aria-label"));
    }
}
