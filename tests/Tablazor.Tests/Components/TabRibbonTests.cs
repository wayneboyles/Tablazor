using Tablazor.Components;
using Tablazor.Enums;
using Tablazor.Icons;

namespace Tablazor.Components;

public class TabRibbonTests : BunitContext
{
    public TabRibbonTests()
    {
        Services.AddLogging();
    }

    [Fact]
    public void HasRibbonClass_ByDefault()
    {
        var cut = Render<TabRibbon>();

        var element = cut.Find("div");
        Assert.Contains("ribbon", element.GetAttribute("class"));
    }

    [Fact]
    public void AppliesTopEndPositionClasses_ByDefault()
    {
        var cut = Render<TabRibbon>();

        var element = cut.Find("div");
        var classes = element.GetAttribute("class");
        Assert.Contains("ribbon-top", classes);
        Assert.Contains("ribbon-end", classes);
    }

    [Theory]
    [InlineData(RibbonPosition.TopEnd, "ribbon-top", "ribbon-end")]
    [InlineData(RibbonPosition.TopStart, "ribbon-top", "ribbon-start")]
    [InlineData(RibbonPosition.BottomEnd, "ribbon-bottom", "ribbon-end")]
    [InlineData(RibbonPosition.BottomStart, "ribbon-bottom", "ribbon-start")]
    public void AppliesCorrectPositionClasses_ForCornerPositions(RibbonPosition position, string expectedClass1, string expectedClass2)
    {
        var cut = Render<TabRibbon>(parameters => parameters
            .Add(p => p.Position, position));

        var classes = cut.Find("div").GetAttribute("class");
        Assert.Contains(expectedClass1, classes);
        Assert.Contains(expectedClass2, classes);
    }

    [Theory]
    [InlineData(RibbonPosition.Top, "ribbon-top")]
    [InlineData(RibbonPosition.Bottom, "ribbon-bottom")]
    [InlineData(RibbonPosition.Start, "ribbon-start")]
    [InlineData(RibbonPosition.End, "ribbon-end")]
    public void AppliesCorrectPositionClass_ForSingleEdgePositions(RibbonPosition position, string expectedClass)
    {
        var cut = Render<TabRibbon>(parameters => parameters
            .Add(p => p.Position, position));

        Assert.Contains(expectedClass, cut.Find("div").GetAttribute("class"));
    }

    [Fact]
    public void AppliesColorClasses_WhenColorIsSet()
    {
        var cut = Render<TabRibbon>(parameters => parameters
            .Add(p => p.Color, TabColors.Primary));

        var classes = cut.Find("div").GetAttribute("class");
        Assert.Contains("bg-primary", classes);
        Assert.Contains("text-primary-fg", classes);
    }

    [Fact]
    public void DoesNotApplyColorClasses_WhenColorIsDefault()
    {
        var cut = Render<TabRibbon>(parameters => parameters
            .Add(p => p.Color, TabColors.Default));

        var classes = cut.Find("div").GetAttribute("class");
        Assert.DoesNotContain("bg-", classes);
        Assert.DoesNotContain("text-", classes);
    }

    [Fact]
    public void AppliesColorClasses_ForDangerColor()
    {
        var cut = Render<TabRibbon>(parameters => parameters
            .Add(p => p.Color, TabColors.Danger));

        var classes = cut.Find("div").GetAttribute("class");
        Assert.Contains("bg-danger", classes);
        Assert.Contains("text-danger-fg", classes);
    }

    [Fact]
    public void AppliesBookmarkClass_WhenBookmarkIsTrue()
    {
        var cut = Render<TabRibbon>(parameters => parameters
            .Add(p => p.Bookmark, true));

        Assert.Contains("ribbon-bookmark", cut.Find("div").GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyBookmarkClass_WhenBookmarkIsFalse()
    {
        var cut = Render<TabRibbon>(parameters => parameters
            .Add(p => p.Bookmark, false));

        Assert.DoesNotContain("ribbon-bookmark", cut.Find("div").GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyBookmarkClass_ByDefault()
    {
        var cut = Render<TabRibbon>();

        Assert.DoesNotContain("ribbon-bookmark", cut.Find("div").GetAttribute("class"));
    }

    [Fact]
    public void RendersChildContent()
    {
        var cut = Render<TabRibbon>(parameters => parameters
            .AddChildContent("NEW"));

        cut.Find("div").MarkupMatches("<div diff:ignoreAttributes>NEW</div>");
    }

    [Fact]
    public void RendersIcon_WhenIconIsSet()
    {
        var cut = Render<TabRibbon>(parameters => parameters
            .Add(p => p.Icon, TabIcons.Outline.Award));

        // TabIcon renders an SVG element
        Assert.NotNull(cut.Find("svg"));
    }

    [Fact]
    public void DoesNotRenderIcon_WhenIconIsNull()
    {
        var cut = Render<TabRibbon>(parameters => parameters
            .Add(p => p.Icon, (string?)null)
            .AddChildContent("NEW"));

        Assert.Empty(cut.FindAll("svg"));
    }

    [Fact]
    public void DoesNotRenderIcon_WhenIconIsEmpty()
    {
        var cut = Render<TabRibbon>(parameters => parameters
            .Add(p => p.Icon, string.Empty)
            .AddChildContent("NEW"));

        Assert.Empty(cut.FindAll("svg"));
    }

    [Fact]
    public void DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabRibbon>(parameters => parameters
            .Add(p => p.Visible, false)
            .AddChildContent("NEW"));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void AppliesAdditionalCssClass_WhenCssClassIsSet()
    {
        var cut = Render<TabRibbon>(parameters => parameters
            .AddUnmatched("class", "my-custom-class"));

        Assert.Contains("my-custom-class", cut.Find("div").GetAttribute("class"));
    }

    [Fact]
    public void AppliesAdditionalAttributes()
    {
        var cut = Render<TabRibbon>(parameters => parameters
            .AddUnmatched("data-testid", "my-ribbon")
            .AddUnmatched("aria-label", "Featured"));

        var element = cut.Find("div");
        Assert.Equal("my-ribbon", element.GetAttribute("data-testid"));
        Assert.Equal("Featured", element.GetAttribute("aria-label"));
    }

    [Fact]
    public void GeneratesUniqueId_WhenNoIdProvided()
    {
        var cut = Render<TabRibbon>();

        var id = cut.Find("div").GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("tab-", id);
    }

    [Fact]
    public void UsesProvidedId_WhenIdIsSet()
    {
        var cut = Render<TabRibbon>(parameters => parameters
            .AddUnmatched("id", "custom-ribbon-id"));

        Assert.Equal("custom-ribbon-id", cut.Find("div").GetAttribute("id"));
    }

    [Fact]
    public void CombinesAllModifiers_WhenAllParametersAreSet()
    {
        var cut = Render<TabRibbon>(parameters => parameters
            .Add(p => p.Color, TabColors.Success)
            .Add(p => p.Position, RibbonPosition.TopStart)
            .Add(p => p.Bookmark, true)
            .AddChildContent("SALE"));

        var classes = cut.Find("div").GetAttribute("class");
        Assert.Contains("ribbon", classes);
        Assert.Contains("ribbon-top", classes);
        Assert.Contains("ribbon-start", classes);
        Assert.Contains("bg-success", classes);
        Assert.Contains("text-success-fg", classes);
        Assert.Contains("ribbon-bookmark", classes);
    }
}
