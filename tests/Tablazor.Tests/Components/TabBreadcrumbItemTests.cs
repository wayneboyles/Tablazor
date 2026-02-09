using Tablazor.Components;

namespace Tablazor.Components;

public class TabBreadcrumbItemTests : BunitContext
{
    public TabBreadcrumbItemTests()
    {
        Services.AddLogging();
    }

    [Fact]
    public void RendersLiElement()
    {
        var cut = Render<TabBreadcrumbItem>(parameters => parameters
            .Add(p => p.Text, "Test"));

        var element = cut.Find("li");
        Assert.NotNull(element);
    }

    [Fact]
    public void HasBreadcrumbItemClass_ByDefault()
    {
        var cut = Render<TabBreadcrumbItem>(parameters => parameters
            .Add(p => p.Text, "Test"));

        var element = cut.Find("li");
        Assert.Contains("breadcrumb-item", element.GetAttribute("class"));
    }

    [Fact]
    public void RendersAsLink_WhenHrefProvided_AndNotActive()
    {
        var cut = Render<TabBreadcrumbItem>(parameters => parameters
            .Add(p => p.Text, "Link")
            .Add(p => p.Href, "/test")
            .Add(p => p.IsActive, false));

        var anchor = cut.Find("a");
        Assert.NotNull(anchor);
        Assert.Equal("/test", anchor.GetAttribute("href"));
    }

    [Fact]
    public void RendersAsText_WhenNoHrefProvided()
    {
        var cut = Render<TabBreadcrumbItem>(parameters => parameters
            .Add(p => p.Text, "Plain Text"));

        var li = cut.Find("li");
        Assert.Empty(li.QuerySelectorAll("a"));
        Assert.Contains("Plain Text", li.TextContent);
    }

    [Fact]
    public void RendersAsText_WhenActiveEvenWithHref()
    {
        var cut = Render<TabBreadcrumbItem>(parameters => parameters
            .Add(p => p.Text, "Active Item")
            .Add(p => p.Href, "/test")
            .Add(p => p.IsActive, true));

        var li = cut.Find("li");
        Assert.Empty(li.QuerySelectorAll("a"));
        Assert.Contains("Active Item", li.TextContent);
    }

    [Fact]
    public void AppliesActiveClass_WhenIsActiveIsTrue()
    {
        var cut = Render<TabBreadcrumbItem>(parameters => parameters
            .Add(p => p.Text, "Active")
            .Add(p => p.IsActive, true));

        var element = cut.Find("li");
        Assert.Contains("active", element.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyActiveClass_WhenIsActiveIsFalse()
    {
        var cut = Render<TabBreadcrumbItem>(parameters => parameters
            .Add(p => p.Text, "Not Active")
            .Add(p => p.IsActive, false));

        var element = cut.Find("li");
        Assert.DoesNotContain("active", element.GetAttribute("class"));
    }

    [Fact]
    public void HasAriaCurrent_WhenIsActive()
    {
        var cut = Render<TabBreadcrumbItem>(parameters => parameters
            .Add(p => p.Text, "Current")
            .Add(p => p.IsActive, true));

        var element = cut.Find("li");
        Assert.Equal("page", element.GetAttribute("aria-current"));
    }

    [Fact]
    public void DoesNotHaveAriaCurrent_WhenNotActive()
    {
        var cut = Render<TabBreadcrumbItem>(parameters => parameters
            .Add(p => p.Text, "Not Current")
            .Add(p => p.IsActive, false));

        var element = cut.Find("li");
        Assert.Null(element.GetAttribute("aria-current"));
    }

    [Fact]
    public void RendersChildContent_WhenProvided()
    {
        var cut = Render<TabBreadcrumbItem>(parameters => parameters
            .AddChildContent("<strong>Bold Text</strong>"));

        cut.Find("li").MarkupMatches("<li diff:ignoreAttributes><strong>Bold Text</strong></li>");
    }

    [Fact]
    public void ChildContent_TakesPrecedence_OverText()
    {
        var cut = Render<TabBreadcrumbItem>(parameters => parameters
            .Add(p => p.Text, "Text Parameter")
            .AddChildContent("<em>Child Content</em>"));

        var li = cut.Find("li");
        Assert.Contains("Child Content", li.TextContent);
        Assert.DoesNotContain("Text Parameter", li.TextContent);
    }

    [Fact]
    public void DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabBreadcrumbItem>(parameters => parameters
            .Add(p => p.Visible, false)
            .Add(p => p.Text, "Hidden"));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void GeneratesUniqueId_WhenNoIdProvided()
    {
        var cut = Render<TabBreadcrumbItem>(parameters => parameters
            .Add(p => p.Text, "Test"));

        var element = cut.Find("li");
        var id = element.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("tab-", id);
    }

    [Fact]
    public void UsesProvidedId_WhenIdIsSet()
    {
        var cut = Render<TabBreadcrumbItem>(parameters => parameters
            .Add(p => p.Text, "Test")
            .AddUnmatched("id", "custom-item-id"));

        var element = cut.Find("li");
        Assert.Equal("custom-item-id", element.GetAttribute("id"));
    }

    [Fact]
    public void AppliesCssClass_WhenProvided()
    {
        var cut = Render<TabBreadcrumbItem>(parameters => parameters
            .Add(p => p.Text, "Test")
            .Add(p => p.CssClass, "custom-class"));

        var element = cut.Find("li");
        var classes = element.GetAttribute("class");
        Assert.Contains("breadcrumb-item", classes);
        Assert.Contains("custom-class", classes);
    }

    [Fact]
    public void InvokesOnClick_WhenLinkIsClicked()
    {
        var clicked = false;
        var cut = Render<TabBreadcrumbItem>(parameters => parameters
            .Add(p => p.Text, "Clickable")
            .Add(p => p.Href, "/test")
            .Add(p => p.OnClick, () => clicked = true));

        var anchor = cut.Find("a");
        anchor.Click();

        Assert.True(clicked);
    }

    [Fact]
    public void PreventsDefaultNavigation_WhenOnClickIsSet()
    {
        var cut = Render<TabBreadcrumbItem>(parameters => parameters
            .Add(p => p.Text, "Clickable")
            .Add(p => p.Href, "/test")
            .Add(p => p.OnClick, () => { }));

        var anchor = cut.Find("a");
        // The onclick:preventDefault should be set when OnClick has a delegate
        Assert.True(anchor.HasAttribute("onclick:preventDefault") ||
                    cut.Markup.Contains("onclick:preventDefault"));
    }
}
