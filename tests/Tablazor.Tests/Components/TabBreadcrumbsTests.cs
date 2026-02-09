using System.Collections.Generic;
using System.Linq;
using Tablazor.Components;
using Tablazor.Enums;

namespace Tablazor.Components;

public class TabBreadcrumbsTests : BunitContext
{
    public TabBreadcrumbsTests()
    {
        Services.AddLogging();
    }

    [Fact]
    public void RendersOlElement()
    {
        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, false));

        var element = cut.Find("ol");
        Assert.NotNull(element);
    }

    [Fact]
    public void HasBreadcrumbClass_ByDefault()
    {
        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, false));

        var element = cut.Find("ol");
        Assert.Contains("breadcrumb", element.GetAttribute("class"));
    }

    [Fact]
    public void AppliesArrowsSeparatorClass_WhenSeparatorIsArrows()
    {
        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, false)
            .Add(p => p.Separator, BreadcrumbSeparator.Arrows));

        var element = cut.Find("ol");
        Assert.Contains("breadcrumb-arrows", element.GetAttribute("class"));
    }

    [Fact]
    public void AppliesDotsSeparatorClass_WhenSeparatorIsDots()
    {
        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, false)
            .Add(p => p.Separator, BreadcrumbSeparator.Dots));

        var element = cut.Find("ol");
        Assert.Contains("breadcrumb-dots", element.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplySeparatorClass_WhenSeparatorIsDefault()
    {
        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, false)
            .Add(p => p.Separator, BreadcrumbSeparator.Default));

        var element = cut.Find("ol");
        var classes = element.GetAttribute("class");
        Assert.DoesNotContain("breadcrumb-arrows", classes);
        Assert.DoesNotContain("breadcrumb-dots", classes);
    }

    [Fact]
    public void RendersStaticItems_WhenItemsProvided()
    {
        var items = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Products", "/products"),
            new("Details", null, true)
        };

        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.AutoGenerate, false));

        var listItems = cut.FindAll("li");
        Assert.Equal(3, listItems.Count);
    }

    [Fact]
    public void RendersChildContent_WhenProvided()
    {
        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, false)
            .AddChildContent<TabBreadcrumbItem>(p => p
                .Add(x => x.Text, "Home")
                .Add(x => x.Href, "/")));

        var listItem = cut.Find("li");
        Assert.NotNull(listItem);
        Assert.Contains("breadcrumb-item", listItem.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.Visible, false)
            .Add(p => p.AutoGenerate, false));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void HasAriaLabel_ForAccessibility()
    {
        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, false));

        var element = cut.Find("ol");
        Assert.Equal("breadcrumb", element.GetAttribute("aria-label"));
    }

    [Fact]
    public void GeneratesUniqueId_WhenNoIdProvided()
    {
        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, false));

        var element = cut.Find("ol");
        var id = element.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("tab-", id);
    }

    [Fact]
    public void UsesProvidedId_WhenIdIsSet()
    {
        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, false)
            .AddUnmatched("id", "custom-breadcrumb-id"));

        var element = cut.Find("ol");
        Assert.Equal("custom-breadcrumb-id", element.GetAttribute("id"));
    }

    [Fact]
    public void AppliesAdditionalAttributes()
    {
        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, false)
            .AddUnmatched("data-testid", "my-breadcrumbs"));

        var element = cut.Find("ol");
        Assert.Equal("my-breadcrumbs", element.GetAttribute("data-testid"));
    }

    [Fact]
    public void AutoGenerates_HomeBreadcrumb_WhenOnRootPage()
    {
        // Use BunitNavigationManager to set the current URI
        var navMan = Services.GetRequiredService<BunitNavigationManager>();
        navMan.NavigateTo("/");

        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, true)
            .Add(p => p.ShowHome, true));

        var listItems = cut.FindAll("li");
        Assert.Single(listItems);
        Assert.Contains("Home", listItems[0].TextContent);
    }

    [Fact]
    public void AutoGenerates_MultipleSegments_FromUrl()
    {
        var navMan = Services.GetRequiredService<BunitNavigationManager>();
        navMan.NavigateTo("/products/category/item");

        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, true)
            .Add(p => p.ShowHome, true));

        var listItems = cut.FindAll("li");
        Assert.Equal(4, listItems.Count); // Home, Products, Category, Item
    }

    [Fact]
    public void AutoGenerate_TransformsSegments_ToTitleCase()
    {
        var navMan = Services.GetRequiredService<BunitNavigationManager>();
        navMan.NavigateTo("/my-products");

        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, true)
            .Add(p => p.ShowHome, true));

        var listItems = cut.FindAll("li");
        Assert.Contains("My Products", listItems[1].TextContent);
    }

    [Fact]
    public void AutoGenerate_UsesCustomTransformer_WhenProvided()
    {
        var navMan = Services.GetRequiredService<BunitNavigationManager>();
        navMan.NavigateTo("/products");

        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, true)
            .Add(p => p.ShowHome, true)
            .Add(p => p.SegmentTransformer, segment => segment.ToUpperInvariant()));

        var listItems = cut.FindAll("li");
        Assert.Contains("PRODUCTS", listItems[1].TextContent);
    }

    [Fact]
    public void AutoGenerate_ExcludesSegments_WhenExcludedSegmentsProvided()
    {
        var navMan = Services.GetRequiredService<BunitNavigationManager>();
        navMan.NavigateTo("/products/123/details");

        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, true)
            .Add(p => p.ShowHome, true)
            .Add(p => p.ExcludedSegments, new[] { "123" }));

        var listItems = cut.FindAll("li");
        Assert.Equal(3, listItems.Count); // Home, Products, Details (not 123)
    }

    [Fact]
    public void AutoGenerate_UsesCustomHomeText_WhenProvided()
    {
        var navMan = Services.GetRequiredService<BunitNavigationManager>();
        navMan.NavigateTo("/");

        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, true)
            .Add(p => p.ShowHome, true)
            .Add(p => p.HomeText, "Dashboard"));

        var listItems = cut.FindAll("li");
        Assert.Contains("Dashboard", listItems[0].TextContent);
    }

    [Fact]
    public void AutoGenerate_HidesHome_WhenShowHomeIsFalse()
    {
        var navMan = Services.GetRequiredService<BunitNavigationManager>();
        navMan.NavigateTo("/products");

        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, true)
            .Add(p => p.ShowHome, false));

        var listItems = cut.FindAll("li");
        Assert.Single(listItems);
        Assert.Contains("Products", listItems[0].TextContent);
    }

    [Fact]
    public void LastItem_IsActive_AndNotALink()
    {
        var navMan = Services.GetRequiredService<BunitNavigationManager>();
        navMan.NavigateTo("/products/details");

        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, true)
            .Add(p => p.ShowHome, true));

        var listItems = cut.FindAll("li");
        var lastItem = listItems.Last();

        Assert.Contains("active", lastItem.GetAttribute("class"));
        Assert.Equal("page", lastItem.GetAttribute("aria-current"));
        Assert.Empty(lastItem.QuerySelectorAll("a"));
    }

    [Fact]
    public void NonLastItems_AreLinks()
    {
        var navMan = Services.GetRequiredService<BunitNavigationManager>();
        navMan.NavigateTo("/products/details");

        var cut = Render<TabBreadcrumbs>(parameters => parameters
            .Add(p => p.AutoGenerate, true)
            .Add(p => p.ShowHome, true));

        var listItems = cut.FindAll("li");

        // Home item should have a link
        var homeLink = listItems[0].QuerySelector("a");
        Assert.NotNull(homeLink);
        Assert.Equal("/", homeLink!.GetAttribute("href"));

        // Products item should have a link
        var productsLink = listItems[1].QuerySelector("a");
        Assert.NotNull(productsLink);
        Assert.Equal("/products", productsLink!.GetAttribute("href"));
    }
}
