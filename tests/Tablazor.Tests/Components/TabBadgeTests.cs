using Tablazor.Components;
using Tablazor.Enums;

namespace Tablazor.Components;

public class TabBadgeTests : BunitContext
{
    public TabBadgeTests()
    {
        Services.AddLogging();
    }

    [Fact]
    public void RendersAsSpan_WhenNoHrefProvided()
    {
        var cut = Render<TabBadge>(parameters => parameters
            .AddChildContent("Test"));

        cut.Find("span").MarkupMatches("<span diff:ignoreAttributes>Test</span>");
    }

    [Fact]
    public void RendersAsAnchor_WhenHrefProvided()
    {
        var cut = Render<TabBadge>(parameters => parameters
            .Add(p => p.Href, "https://example.com")
            .AddChildContent("Link"));

        var anchor = cut.Find("a");
        Assert.Equal("https://example.com", anchor.GetAttribute("href"));
    }

    [Fact]
    public void HasBadgeClass_ByDefault()
    {
        var cut = Render<TabBadge>();

        var element = cut.Find("span");
        Assert.Contains("badge", element.GetAttribute("class"));
    }

    [Fact]
    public void AppliesColorClasses_WhenColorIsSet()
    {
        var cut = Render<TabBadge>(parameters => parameters
            .Add(p => p.Color, TabColors.Primary));

        var element = cut.Find("span");
        var classes = element.GetAttribute("class");
        Assert.Contains("bg-primary", classes);
        Assert.Contains("text-primary-fg", classes);
    }

    [Fact]
    public void DoesNotApplyColorClasses_WhenColorIsDefault()
    {
        var cut = Render<TabBadge>(parameters => parameters
            .Add(p => p.Color, TabColors.Default));

        var element = cut.Find("span");
        var classes = element.GetAttribute("class");
        Assert.DoesNotContain("bg-", classes);
        Assert.DoesNotContain("text-", classes);
    }

    [Fact]
    public void AppliesPillClass_WhenShapeIsPill()
    {
        var cut = Render<TabBadge>(parameters => parameters
            .Add(p => p.Shape, BadgeShape.Pill));

        var element = cut.Find("span");
        Assert.Contains("badge-pill", element.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyPillClass_WhenShapeIsDefault()
    {
        var cut = Render<TabBadge>(parameters => parameters
            .Add(p => p.Shape, BadgeShape.Default));

        var element = cut.Find("span");
        Assert.DoesNotContain("badge-pill", element.GetAttribute("class"));
    }

    [Fact]
    public void AppliesSizeClass_WhenSizeIsSmall()
    {
        var cut = Render<TabBadge>(parameters => parameters
            .Add(p => p.Size, BadgeSize.Small));

        var element = cut.Find("span");
        Assert.Contains("badge-sm", element.GetAttribute("class"));
    }

    [Fact]
    public void AppliesSizeClass_WhenSizeIsLarge()
    {
        var cut = Render<TabBadge>(parameters => parameters
            .Add(p => p.Size, BadgeSize.Large));

        var element = cut.Find("span");
        Assert.Contains("badge-lg", element.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplySizeClass_WhenSizeIsDefault()
    {
        var cut = Render<TabBadge>(parameters => parameters
            .Add(p => p.Size, BadgeSize.Default));

        var element = cut.Find("span");
        Assert.DoesNotContain("badge-sm", element.GetAttribute("class"));
        Assert.DoesNotContain("badge-lg", element.GetAttribute("class"));
    }

    [Fact]
    public void RendersChildContent()
    {
        var cut = Render<TabBadge>(parameters => parameters
            .AddChildContent("<strong>Bold Text</strong>"));

        cut.Find("span").MarkupMatches("<span diff:ignoreAttributes><strong>Bold Text</strong></span>");
    }

    [Fact]
    public void DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabBadge>(parameters => parameters
            .Add(p => p.Visible, false)
            .AddChildContent("Hidden"));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void AppliesStyle_WhenStyleIsSet()
    {
        var cut = Render<TabBadge>(parameters => parameters
            .Add(p => p.Style, "color: red;"));

        var element = cut.Find("span");
        Assert.Equal("color: red;", element.GetAttribute("style"));
    }

    [Fact]
    public void AppliesAdditionalAttributes()
    {
        var cut = Render<TabBadge>(parameters => parameters
            .AddUnmatched("data-testid", "my-badge")
            .AddUnmatched("aria-label", "Badge label"));

        var element = cut.Find("span");
        Assert.Equal("my-badge", element.GetAttribute("data-testid"));
        Assert.Equal("Badge label", element.GetAttribute("aria-label"));
    }

    [Fact]
    public void GeneratesUniqueId_WhenNoIdProvided()
    {
        var cut = Render<TabBadge>();

        var element = cut.Find("span");
        var id = element.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("tab-", id);
    }

    [Fact]
    public void UsesProvidedId_WhenIdIsSet()
    {
        var cut = Render<TabBadge>(parameters => parameters
            .AddUnmatched("id", "custom-badge-id"));

        var element = cut.Find("span");
        Assert.Equal("custom-badge-id", element.GetAttribute("id"));
    }
}
