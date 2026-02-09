using Tablazor.Components;

namespace Tablazor.Components;

public class TabCarouselSlideTests : BunitContext
{
    public TabCarouselSlideTests()
    {
        Services.AddLogging();
    }

    [Fact]
    public void RendersSlideElement()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
            }));

        var slide = cut.Find("div.carousel-item");
        Assert.NotNull(slide);
    }

    [Fact]
    public void RendersImage_WhenImageUrlProvided()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.AddAttribute(1, "ImageUrl", "test-image.jpg");
                builder.CloseComponent();
            }));

        var img = cut.Find("img");
        Assert.Equal("test-image.jpg", img.GetAttribute("src"));
    }

    [Fact]
    public void ImageHasCorrectClasses()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.AddAttribute(1, "ImageUrl", "test-image.jpg");
                builder.CloseComponent();
            }));

        var img = cut.Find("img");
        var classes = img.GetAttribute("class");
        Assert.Contains("d-block", classes);
        Assert.Contains("w-100", classes);
    }

    [Fact]
    public void RendersImageAlt_WhenProvided()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.AddAttribute(1, "ImageUrl", "test-image.jpg");
                builder.AddAttribute(2, "ImageAlt", "Test Image Description");
                builder.CloseComponent();
            }));

        var img = cut.Find("img");
        Assert.Equal("Test Image Description", img.GetAttribute("alt"));
    }

    [Fact]
    public void DoesNotRenderImage_WhenNoImageUrl()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
            }));

        var images = cut.FindAll("img");
        Assert.Empty(images);
    }

    [Fact]
    public void RendersCaption_WhenCaptionTitleProvided()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.AddAttribute(1, "CaptionTitle", "Test Title");
                builder.CloseComponent();
            }));

        var caption = cut.Find("div.carousel-caption");
        Assert.NotNull(caption);
        Assert.Contains("Test Title", caption.TextContent);
    }

    [Fact]
    public void RendersCaptionTitle_AsH3()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.AddAttribute(1, "CaptionTitle", "Test Title");
                builder.CloseComponent();
            }));

        var h3 = cut.Find("div.carousel-caption h3");
        Assert.Equal("Test Title", h3.TextContent);
    }

    [Fact]
    public void RendersCaptionText_AsP()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.AddAttribute(1, "CaptionText", "Test description text");
                builder.CloseComponent();
            }));

        var p = cut.Find("div.carousel-caption p");
        Assert.Equal("Test description text", p.TextContent);
    }

    [Fact]
    public void DoesNotRenderCaption_WhenNoTitleOrText()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.AddAttribute(1, "ImageUrl", "test.jpg");
                builder.CloseComponent();
            }));

        var captions = cut.FindAll("div.carousel-caption");
        Assert.Empty(captions);
    }

    [Fact]
    public void RendersCaptionBackground_WhenEnabled()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.AddAttribute(1, "CaptionTitle", "Test");
                builder.AddAttribute(2, "ShowCaptionBackground", true);
                builder.CloseComponent();
            }));

        var background = cut.Find("div.carousel-caption-background");
        Assert.NotNull(background);
    }

    [Fact]
    public void DoesNotRenderCaptionBackground_ByDefault()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.AddAttribute(1, "CaptionTitle", "Test");
                builder.CloseComponent();
            }));

        var backgrounds = cut.FindAll("div.carousel-caption-background");
        Assert.Empty(backgrounds);
    }

    [Fact]
    public void FirstSlideHasActiveClass()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
            }));

        var slides = cut.FindAll("div.carousel-item");
        Assert.Contains("active", slides[0].GetAttribute("class"));
    }

    [Fact]
    public void SecondSlideDoesNotHaveActiveClass_Initially()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.CloseComponent();
            }));

        var slides = cut.FindAll("div.carousel-item");
        Assert.DoesNotContain("active", slides[1].GetAttribute("class") ?? "");
    }

    [Fact]
    public void RendersChildContent()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b =>
                {
                    b.AddMarkupContent(0, "<div class=\"custom-content\">Custom Content</div>");
                }));
                builder.CloseComponent();
            }));

        var customContent = cut.Find("div.custom-content");
        Assert.Equal("Custom Content", customContent.TextContent);
    }

    [Fact]
    public void DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.AddAttribute(1, "Visible", false);
                builder.CloseComponent();
            }));

        var slides = cut.FindAll("div.carousel-item");
        Assert.Empty(slides);
    }

    [Fact]
    public void AppliesCustomCssClass()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.AddAttribute(1, "CssClass", "my-custom-slide");
                builder.CloseComponent();
            }));

        var slide = cut.Find("div.carousel-item");
        Assert.Contains("my-custom-slide", slide.GetAttribute("class"));
    }

    [Fact]
    public void AppliesStyle()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.AddAttribute(1, "Style", "min-height: 300px;");
                builder.CloseComponent();
            }));

        var slide = cut.Find("div.carousel-item");
        Assert.Equal("min-height: 300px;", slide.GetAttribute("style"));
    }

    [Fact]
    public void CaptionHasResponsiveClasses()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.AddAttribute(1, "CaptionTitle", "Test");
                builder.CloseComponent();
            }));

        var caption = cut.Find("div.carousel-caption");
        var classes = caption.GetAttribute("class");
        Assert.Contains("d-none", classes);
        Assert.Contains("d-md-block", classes);
    }

    [Fact]
    public void RendersThumbnailIndicator_WhenThumbnailStyleAndImageProvided()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.IndicatorStyle, Tablazor.Enums.CarouselIndicatorStyle.Thumbnail)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.AddAttribute(1, "ImageUrl", "image1.jpg");
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.AddAttribute(2, "ImageUrl", "image2.jpg");
                builder.CloseComponent();
            }));

        var indicators = cut.FindAll("div.carousel-indicators button");
        Assert.Equal(2, indicators.Count);
        // Thumbnail buttons should have background-image style
        Assert.Contains("background-image", indicators[0].GetAttribute("style"));
    }

    [Fact]
    public void UsesThumbnailUrl_WhenProvided()
    {
        var cut = Render<TabCarousel>(parameters => parameters
            .Add(p => p.IndicatorStyle, Tablazor.Enums.CarouselIndicatorStyle.Thumbnail)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabCarouselSlide>(0);
                builder.AddAttribute(1, "ImageUrl", "image1.jpg");
                builder.AddAttribute(2, "ThumbnailUrl", "thumb1.jpg");
                builder.CloseComponent();
                builder.OpenComponent<TabCarouselSlide>(1);
                builder.AddAttribute(3, "ImageUrl", "image2.jpg");
                builder.CloseComponent();
            }));

        var indicators = cut.FindAll("div.carousel-indicators button");
        Assert.Contains("thumb1.jpg", indicators[0].GetAttribute("style"));
        Assert.Contains("image2.jpg", indicators[1].GetAttribute("style"));
    }
}
