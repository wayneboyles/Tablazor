using Tablazor.Components;
using Tablazor.Enums;

namespace Tablazor.Tests.Components;

public class TabCardTests : BunitContext
{
    public TabCardTests()
    {
        Services.AddLogging();
    }

    #region TabCard Tests

    [Fact]
    public void HasCardClass_ByDefault()
    {
        var cut = Render<TabCard>();

        var element = cut.Find("div");
        Assert.Contains("card", element.GetAttribute("class"));
    }

    [Fact]
    public void RendersChildContent()
    {
        var cut = Render<TabCard>(parameters => parameters
            .AddChildContent("<p>Test content</p>"));

        cut.Find("div").MarkupMatches("<div diff:ignoreAttributes><p>Test content</p></div>");
    }

    [Fact]
    public void DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabCard>(parameters => parameters
            .Add(p => p.Visible, false)
            .AddChildContent("Hidden"));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void AppliesPaddingClass_WhenPaddingIsSmall()
    {
        var cut = Render<TabCard>(parameters => parameters
            .Add(p => p.Padding, CardPadding.Small));

        var element = cut.Find("div");
        Assert.Contains("card-sm", element.GetAttribute("class"));
    }

    [Fact]
    public void AppliesPaddingClass_WhenPaddingIsMedium()
    {
        var cut = Render<TabCard>(parameters => parameters
            .Add(p => p.Padding, CardPadding.Medium));

        var element = cut.Find("div");
        Assert.Contains("card-md", element.GetAttribute("class"));
    }

    [Fact]
    public void AppliesPaddingClass_WhenPaddingIsLarge()
    {
        var cut = Render<TabCard>(parameters => parameters
            .Add(p => p.Padding, CardPadding.Large));

        var element = cut.Find("div");
        Assert.Contains("card-lg", element.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyPaddingClass_WhenPaddingIsDefault()
    {
        var cut = Render<TabCard>(parameters => parameters
            .Add(p => p.Padding, CardPadding.Default));

        var element = cut.Find("div");
        var classes = element.GetAttribute("class");
        Assert.DoesNotContain("card-sm", classes);
        Assert.DoesNotContain("card-md", classes);
        Assert.DoesNotContain("card-lg", classes);
    }

    [Fact]
    public void AppliesStatusPositionClass_WhenStatusPositionIsTop()
    {
        var cut = Render<TabCard>(parameters => parameters
            .Add(p => p.StatusPosition, CardStatusPosition.Top));

        var element = cut.Find("div");
        Assert.Contains("card-status-top", element.GetAttribute("class"));
    }

    [Fact]
    public void AppliesStatusPositionClass_WhenStatusPositionIsStart()
    {
        var cut = Render<TabCard>(parameters => parameters
            .Add(p => p.StatusPosition, CardStatusPosition.Start));

        var element = cut.Find("div");
        Assert.Contains("card-status-start", element.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyStatusPositionClass_WhenStatusPositionIsNone()
    {
        var cut = Render<TabCard>(parameters => parameters
            .Add(p => p.StatusPosition, CardStatusPosition.None));

        var element = cut.Find("div");
        var classes = element.GetAttribute("class");
        Assert.DoesNotContain("card-status-top", classes);
        Assert.DoesNotContain("card-status-start", classes);
    }

    [Fact]
    public void AppliesStatusColorClass_WhenStatusPositionIsSetAndColorIsSet()
    {
        var cut = Render<TabCard>(parameters => parameters
            .Add(p => p.StatusPosition, CardStatusPosition.Top)
            .Add(p => p.StatusColor, TabColors.Primary));

        var element = cut.Find("div");
        Assert.Contains("bg-primary", element.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyStatusColorClass_WhenStatusPositionIsNone()
    {
        var cut = Render<TabCard>(parameters => parameters
            .Add(p => p.StatusPosition, CardStatusPosition.None)
            .Add(p => p.StatusColor, TabColors.Primary));

        var element = cut.Find("div");
        Assert.DoesNotContain("bg-primary", element.GetAttribute("class"));
    }

    [Fact]
    public void AppliesStackedClass_WhenStackedIsTrue()
    {
        var cut = Render<TabCard>(parameters => parameters
            .Add(p => p.Stacked, true));

        var element = cut.Find("div");
        Assert.Contains("card-stacked", element.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyStackedClass_WhenStackedIsFalse()
    {
        var cut = Render<TabCard>(parameters => parameters
            .Add(p => p.Stacked, false));

        var element = cut.Find("div");
        Assert.DoesNotContain("card-stacked", element.GetAttribute("class"));
    }

    [Fact]
    public void AppliesBorderlessClass_WhenBorderlessIsTrue()
    {
        var cut = Render<TabCard>(parameters => parameters
            .Add(p => p.Borderless, true));

        var element = cut.Find("div");
        Assert.Contains("card-borderless", element.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyBorderlessClass_WhenBorderlessIsFalse()
    {
        var cut = Render<TabCard>(parameters => parameters
            .Add(p => p.Borderless, false));

        var element = cut.Find("div");
        Assert.DoesNotContain("card-borderless", element.GetAttribute("class"));
    }

    [Fact]
    public void GeneratesUniqueId_WhenNoIdProvided()
    {
        var cut = Render<TabCard>();

        var element = cut.Find("div");
        var id = element.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("tab-", id);
    }

    [Fact]
    public void UsesProvidedId_WhenIdIsSet()
    {
        var cut = Render<TabCard>(parameters => parameters
            .AddUnmatched("id", "custom-card-id"));

        var element = cut.Find("div");
        Assert.Equal("custom-card-id", element.GetAttribute("id"));
    }

    [Fact]
    public void AppliesAdditionalAttributes()
    {
        var cut = Render<TabCard>(parameters => parameters
            .AddUnmatched("data-testid", "my-card")
            .AddUnmatched("aria-label", "Card label"));

        var element = cut.Find("div");
        Assert.Equal("my-card", element.GetAttribute("data-testid"));
        Assert.Equal("Card label", element.GetAttribute("aria-label"));
    }

    [Fact]
    public void AppliesCssClass_WhenCssClassIsSet()
    {
        var cut = Render<TabCard>(parameters => parameters
            .AddUnmatched("class", "custom-class"));

        var element = cut.Find("div");
        Assert.Contains("custom-class", element.GetAttribute("class"));
    }

    #endregion

    #region TabCardHeader Tests

    [Fact]
    public void TabCardHeader_HasCardHeaderClass()
    {
        var cut = Render<TabCardHeader>();

        var element = cut.Find("div");
        Assert.Contains("card-header", element.GetAttribute("class"));
    }

    [Fact]
    public void TabCardHeader_RendersChildContent()
    {
        var cut = Render<TabCardHeader>(parameters => parameters
            .AddChildContent("<span>Header content</span>"));

        cut.Find("div").MarkupMatches("<div diff:ignoreAttributes><span>Header content</span></div>");
    }

    [Fact]
    public void TabCardHeader_DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabCardHeader>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    #endregion

    #region TabCardBody Tests

    [Fact]
    public void TabCardBody_HasCardBodyClass()
    {
        var cut = Render<TabCardBody>();

        var element = cut.Find("div");
        Assert.Contains("card-body", element.GetAttribute("class"));
    }

    [Fact]
    public void TabCardBody_RendersChildContent()
    {
        var cut = Render<TabCardBody>(parameters => parameters
            .AddChildContent("<p>Body content</p>"));

        cut.Find("div").MarkupMatches("<div diff:ignoreAttributes><p>Body content</p></div>");
    }

    [Fact]
    public void TabCardBody_DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabCardBody>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    #endregion

    #region TabCardFooter Tests

    [Fact]
    public void TabCardFooter_HasCardFooterClass()
    {
        var cut = Render<TabCardFooter>();

        var element = cut.Find("div");
        Assert.Contains("card-footer", element.GetAttribute("class"));
    }

    [Fact]
    public void TabCardFooter_RendersChildContent()
    {
        var cut = Render<TabCardFooter>(parameters => parameters
            .AddChildContent("<button>Action</button>"));

        cut.Find("div").MarkupMatches("<div diff:ignoreAttributes><button>Action</button></div>");
    }

    [Fact]
    public void TabCardFooter_DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabCardFooter>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    #endregion

    #region TabCardTitle Tests

    [Fact]
    public void TabCardTitle_HasCardTitleClass()
    {
        var cut = Render<TabCardTitle>();

        var element = cut.Find("h3");
        Assert.Contains("card-title", element.GetAttribute("class"));
    }

    [Fact]
    public void TabCardTitle_RendersAsH3_ByDefault()
    {
        var cut = Render<TabCardTitle>(parameters => parameters
            .AddChildContent("Title"));

        Assert.NotNull(cut.Find("h3"));
    }

    [Fact]
    public void TabCardTitle_RendersAsH1_WhenLevelIs1()
    {
        var cut = Render<TabCardTitle>(parameters => parameters
            .Add(p => p.Level, 1)
            .AddChildContent("Title"));

        Assert.NotNull(cut.Find("h1"));
    }

    [Fact]
    public void TabCardTitle_RendersAsH2_WhenLevelIs2()
    {
        var cut = Render<TabCardTitle>(parameters => parameters
            .Add(p => p.Level, 2)
            .AddChildContent("Title"));

        Assert.NotNull(cut.Find("h2"));
    }

    [Fact]
    public void TabCardTitle_RendersAsH4_WhenLevelIs4()
    {
        var cut = Render<TabCardTitle>(parameters => parameters
            .Add(p => p.Level, 4)
            .AddChildContent("Title"));

        Assert.NotNull(cut.Find("h4"));
    }

    [Fact]
    public void TabCardTitle_RendersAsH5_WhenLevelIs5()
    {
        var cut = Render<TabCardTitle>(parameters => parameters
            .Add(p => p.Level, 5)
            .AddChildContent("Title"));

        Assert.NotNull(cut.Find("h5"));
    }

    [Fact]
    public void TabCardTitle_RendersAsH6_WhenLevelIs6()
    {
        var cut = Render<TabCardTitle>(parameters => parameters
            .Add(p => p.Level, 6)
            .AddChildContent("Title"));

        Assert.NotNull(cut.Find("h6"));
    }

    [Fact]
    public void TabCardTitle_RendersChildContent()
    {
        var cut = Render<TabCardTitle>(parameters => parameters
            .AddChildContent("My Card Title"));

        cut.Find("h3").MarkupMatches("<h3 diff:ignoreAttributes>My Card Title</h3>");
    }

    [Fact]
    public void TabCardTitle_DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabCardTitle>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    #endregion

    #region TabCardSubtitle Tests

    [Fact]
    public void TabCardSubtitle_HasCardSubtitleClass()
    {
        var cut = Render<TabCardSubtitle>();

        var element = cut.Find("p");
        Assert.Contains("card-subtitle", element.GetAttribute("class"));
    }

    [Fact]
    public void TabCardSubtitle_RendersAsParagraph()
    {
        var cut = Render<TabCardSubtitle>(parameters => parameters
            .AddChildContent("Subtitle"));

        Assert.NotNull(cut.Find("p"));
    }

    [Fact]
    public void TabCardSubtitle_RendersChildContent()
    {
        var cut = Render<TabCardSubtitle>(parameters => parameters
            .AddChildContent("My Card Subtitle"));

        cut.Find("p").MarkupMatches("<p diff:ignoreAttributes>My Card Subtitle</p>");
    }

    [Fact]
    public void TabCardSubtitle_DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabCardSubtitle>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    #endregion

    #region TabCardActions Tests

    [Fact]
    public void TabCardActions_HasCardActionsClass()
    {
        var cut = Render<TabCardActions>();

        var element = cut.Find("div");
        Assert.Contains("card-actions", element.GetAttribute("class"));
    }

    [Fact]
    public void TabCardActions_RendersChildContent()
    {
        var cut = Render<TabCardActions>(parameters => parameters
            .AddChildContent("<button class=\"btn\">Click</button>"));

        cut.Find("div").MarkupMatches("<div diff:ignoreAttributes><button class=\"btn\">Click</button></div>");
    }

    [Fact]
    public void TabCardActions_DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabCardActions>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void TabCard_CombinesMultipleClassOptions()
    {
        var cut = Render<TabCard>(parameters => parameters
            .Add(p => p.Padding, CardPadding.Small)
            .Add(p => p.StatusPosition, CardStatusPosition.Top)
            .Add(p => p.StatusColor, TabColors.Success)
            .Add(p => p.Stacked, true));

        var element = cut.Find("div");
        var classes = element.GetAttribute("class");
        Assert.Contains("card", classes);
        Assert.Contains("card-sm", classes);
        Assert.Contains("card-status-top", classes);
        Assert.Contains("bg-success", classes);
        Assert.Contains("card-stacked", classes);
    }

    [Fact]
    public void TabCard_WithNestedComponents_RendersCorrectly()
    {
        var cut = Render<TabCard>(parameters => parameters
            .AddChildContent("<div class=\"card-header\"><h3 class=\"card-title\">Test</h3></div><div class=\"card-body\">Content</div>"));

        Assert.NotNull(cut.Find(".card-header"));
        Assert.NotNull(cut.Find(".card-title"));
        Assert.NotNull(cut.Find(".card-body"));
    }

    #endregion
}
