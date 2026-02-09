using Tablazor.Components;
using Tablazor.Enums;
using Tablazor.Icons;
using Tablazor.Services;

namespace Tablazor.Components;

public class TabToastTests : BunitContext
{
    public TabToastTests()
    {
        Services.AddLogging();
    }

    [Fact]
    public void RendersToast_WithBasicMarkup()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Title, "Test Title")
            .Add(p => p.Message, "Test Message"));

        var toast = cut.Find(".toast");
        Assert.NotNull(toast);
    }

    [Fact]
    public void RendersToastHeader_WhenTitleProvided()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Title, "My Title"));

        var header = cut.Find(".toast-header");
        Assert.Contains("My Title", header.InnerHtml);
    }

    [Fact]
    public void RendersToastBody_WhenMessageProvided()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Message, "My Message"));

        var body = cut.Find(".toast-body");
        Assert.Contains("My Message", body.InnerHtml);
    }

    [Fact]
    public void RendersCloseButton_WhenClosableIsTrue()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Title, "Title")
            .Add(p => p.Closable, true));

        var closeButton = cut.Find(".btn-close");
        Assert.NotNull(closeButton);
    }

    [Fact]
    public void DoesNotRenderCloseButton_WhenClosableIsFalse()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Title, "Title")
            .Add(p => p.Closable, false));

        Assert.Throws<Bunit.ElementNotFoundException>(() => cut.Find(".btn-close"));
    }

    [Fact]
    public void AppliesColorClass_WhenColorIsSet()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Title, "Title")
            .Add(p => p.Color, TabColors.Success));

        var header = cut.Find(".toast-header");
        Assert.Contains("bg-success", header.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyColorClass_WhenColorIsDefault()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Title, "Title")
            .Add(p => p.Color, TabColors.Default));

        var header = cut.Find(".toast-header");
        Assert.DoesNotContain("bg-", header.GetAttribute("class"));
    }

    [Fact]
    public void RendersIcon_WhenIconProvided()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Title, "Title")
            .Add(p => p.Icon, TabIcons.Filled.CircleCheck));

        var header = cut.Find(".toast-header");
        // TabIcon renders an SVG element
        Assert.Contains("<svg", header.InnerHtml);
    }

    [Fact]
    public void AppliesTranslucentClass_WhenTranslucentIsTrue()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Title, "Title")
            .Add(p => p.Translucent, true));

        var toast = cut.Find(".toast");
        Assert.Contains("toast-translucent", toast.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyTranslucentClass_WhenTranslucentIsFalse()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Title, "Title")
            .Add(p => p.Translucent, false));

        var toast = cut.Find(".toast");
        Assert.DoesNotContain("toast-translucent", toast.GetAttribute("class"));
    }

    [Fact]
    public void HasAriaAttributes_ForAccessibility()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Title, "Title"));

        var toast = cut.Find(".toast");
        Assert.Equal("alert", toast.GetAttribute("role"));
        Assert.Equal("assertive", toast.GetAttribute("aria-live"));
        Assert.Equal("true", toast.GetAttribute("aria-atomic"));
    }

    [Fact]
    public void AppliesFadeClass_WhenAnimationEnabled()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Title, "Title")
            .Add(p => p.EnableAnimation, true));

        var toast = cut.Find(".toast");
        Assert.Contains("fade", toast.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyFadeClass_WhenAnimationDisabled()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Title, "Title")
            .Add(p => p.EnableAnimation, false));

        var toast = cut.Find(".toast");
        Assert.DoesNotContain("fade", toast.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Title, "Title")
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void RendersChildContent_InsteadOfMessage()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Message, "Plain Message")
            .AddChildContent("<strong>Rich Content</strong>"));

        var body = cut.Find(".toast-body");
        Assert.Contains("<strong>Rich Content</strong>", body.InnerHtml);
        Assert.DoesNotContain("Plain Message", body.InnerHtml);
    }

    [Fact]
    public void AppliesCustomCssClass()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Title, "Title")
            .Add(p => p.CssClass, "my-custom-class"));

        var toast = cut.Find(".toast");
        Assert.Contains("my-custom-class", toast.GetAttribute("class"));
    }

    [Fact]
    public void AppliesWhiteCloseButton_ForDarkColors()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Title, "Title")
            .Add(p => p.Color, TabColors.Danger)
            .Add(p => p.Closable, true));

        var closeButton = cut.Find(".btn-close");
        Assert.Contains("btn-close-white", closeButton.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyWhiteCloseButton_ForLightColor()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Title, "Title")
            .Add(p => p.Color, TabColors.Light)
            .Add(p => p.Closable, true));

        var closeButton = cut.Find(".btn-close");
        Assert.DoesNotContain("btn-close-white", closeButton.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyWhiteCloseButton_ForWarningColor()
    {
        var cut = Render<TabToast>(parameters => parameters
            .Add(p => p.Title, "Title")
            .Add(p => p.Color, TabColors.Warning)
            .Add(p => p.Closable, true));

        var closeButton = cut.Find(".btn-close");
        Assert.DoesNotContain("btn-close-white", closeButton.GetAttribute("class"));
    }
}
