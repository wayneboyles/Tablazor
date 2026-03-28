using Tablazor.Components;
using Tablazor.Enums;

namespace Tablazor.Tests.Components;

public class TabLayoutTests : BunitContext
{
    public TabLayoutTests()
    {
        Services.AddLogging();
    }

    #region TabLayout Tests

    [Fact]
    public void TabLayout_RendersPageDiv()
    {
        var cut = Render<TabLayout>();

        var element = cut.Find("div");
        Assert.Contains("page", element.GetAttribute("class"));
    }

    [Fact]
    public void TabLayout_RendersChildContent()
    {
        var cut = Render<TabLayout>(parameters => parameters
            .AddChildContent("<span>page content</span>"));

        cut.Find("div").MarkupMatches("<div diff:ignoreAttributes><span>page content</span></div>");
    }

    [Fact]
    public void TabLayout_DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabLayout>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void TabLayout_AppendsCssClass_WhenCssClassIsSet()
    {
        var cut = Render<TabLayout>(parameters => parameters
            .Add(p => p.CssClass, "my-layout"));

        var element = cut.Find("div");
        var classes = element.GetAttribute("class");
        Assert.Contains("page", classes);
        Assert.Contains("my-layout", classes);
    }

    [Fact]
    public void TabLayout_GeneratesUniqueId_WhenNoIdProvided()
    {
        var cut = Render<TabLayout>();

        var element = cut.Find("div");
        var id = element.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("tab-", id);
    }

    #endregion

    #region TabNavbar Tests

    [Fact]
    public void TabNavbar_RendersHeaderElement()
    {
        var cut = Render<TabNavbar>();

        Assert.NotNull(cut.Find("header"));
    }

    [Fact]
    public void TabNavbar_HasRequiredClasses()
    {
        var cut = Render<TabNavbar>();

        var element = cut.Find("header");
        var classes = element.GetAttribute("class");
        Assert.Contains("navbar", classes);
        Assert.Contains("navbar-expand-md", classes);
        Assert.Contains("d-print-none", classes);
    }

    [Fact]
    public void TabNavbar_AddsDarkClass_WhenDarkIsTrue()
    {
        var cut = Render<TabNavbar>(parameters => parameters
            .Add(p => p.Dark, true));

        var element = cut.Find("header");
        Assert.Contains("navbar-dark", element.GetAttribute("class"));
    }

    [Fact]
    public void TabNavbar_DoesNotAddDarkClass_WhenDarkIsFalse()
    {
        var cut = Render<TabNavbar>(parameters => parameters
            .Add(p => p.Dark, false));

        var element = cut.Find("header");
        Assert.DoesNotContain("navbar-dark", element.GetAttribute("class"));
    }

    [Fact]
    public void TabNavbar_AddsTransparentClass_WhenTransparentIsTrue()
    {
        var cut = Render<TabNavbar>(parameters => parameters
            .Add(p => p.Transparent, true));

        var element = cut.Find("header");
        Assert.Contains("navbar-transparent", element.GetAttribute("class"));
    }

    [Fact]
    public void TabNavbar_DoesNotAddTransparentClass_WhenTransparentIsFalse()
    {
        var cut = Render<TabNavbar>(parameters => parameters
            .Add(p => p.Transparent, false));

        var element = cut.Find("header");
        Assert.DoesNotContain("navbar-transparent", element.GetAttribute("class"));
    }

    [Fact]
    public void TabNavbar_AddsOverlapClass_WhenOverlapIsTrue()
    {
        var cut = Render<TabNavbar>(parameters => parameters
            .Add(p => p.Overlap, true));

        var element = cut.Find("header");
        Assert.Contains("navbar-overlap", element.GetAttribute("class"));
    }

    [Fact]
    public void TabNavbar_DoesNotAddOverlapClass_WhenOverlapIsFalse()
    {
        var cut = Render<TabNavbar>(parameters => parameters
            .Add(p => p.Overlap, false));

        var element = cut.Find("header");
        Assert.DoesNotContain("navbar-overlap", element.GetAttribute("class"));
    }

    [Fact]
    public void TabNavbar_AddsStickyTopClass_WhenStickyIsTrue()
    {
        var cut = Render<TabNavbar>(parameters => parameters
            .Add(p => p.Sticky, true));

        var element = cut.Find("header");
        Assert.Contains("sticky-top", element.GetAttribute("class"));
    }

    [Fact]
    public void TabNavbar_DoesNotAddStickyTopClass_WhenStickyIsFalse()
    {
        var cut = Render<TabNavbar>(parameters => parameters
            .Add(p => p.Sticky, false));

        var element = cut.Find("header");
        Assert.DoesNotContain("sticky-top", element.GetAttribute("class"));
    }

    [Fact]
    public void TabNavbar_RendersBrand_WhenBrandIsProvided()
    {
        var cut = Render<TabNavbar>(parameters => parameters
            .Add(p => p.Brand, builder =>
            {
                builder.AddMarkupContent(0, "<a href=\"/\">My Brand</a>");
            }));

        Assert.NotNull(cut.Find(".navbar-brand"));
        Assert.Contains("My Brand", cut.Markup);
    }

    [Fact]
    public void TabNavbar_DoesNotRenderBrand_WhenBrandIsNull()
    {
        var cut = Render<TabNavbar>();

        Assert.Throws<Bunit.ElementNotFoundException>(() => cut.Find(".navbar-brand"));
    }

    [Fact]
    public void TabNavbar_RendersNavContent_WhenNavContentIsProvided()
    {
        var cut = Render<TabNavbar>(parameters => parameters
            .Add(p => p.NavContent, builder =>
            {
                builder.AddMarkupContent(0, "<ul class=\"navbar-nav\"><li>Item</li></ul>");
            }));

        Assert.NotNull(cut.Find(".navbar-collapse"));
        Assert.NotNull(cut.Find(".navbar-nav"));
    }

    [Fact]
    public void TabNavbar_DoesNotRenderNavContent_WhenNavContentIsNull()
    {
        var cut = Render<TabNavbar>();

        Assert.Throws<Bunit.ElementNotFoundException>(() => cut.Find(".navbar-collapse"));
    }

    [Fact]
    public void TabNavbar_RendersRightContent_WhenRightContentIsProvided()
    {
        var cut = Render<TabNavbar>(parameters => parameters
            .Add(p => p.RightContent, builder =>
            {
                builder.AddMarkupContent(0, "<span>User Menu</span>");
            }));

        Assert.NotNull(cut.Find(".navbar-nav.flex-row.order-md-last"));
        Assert.Contains("User Menu", cut.Markup);
    }

    [Fact]
    public void TabNavbar_DoesNotRenderRightContent_WhenRightContentIsNull()
    {
        var cut = Render<TabNavbar>();

        Assert.Throws<Bunit.ElementNotFoundException>(() => cut.Find(".order-md-last"));
    }

    [Fact]
    public void TabNavbar_UsesContainerLg_WhenContainerSizeIsLarge()
    {
        var cut = Render<TabNavbar>(parameters => parameters
            .Add(p => p.ContainerSize, ContainerSize.Large));

        Assert.NotNull(cut.Find(".container-lg"));
    }

    [Fact]
    public void TabNavbar_UsesContainerXl_ByDefault()
    {
        var cut = Render<TabNavbar>();

        Assert.NotNull(cut.Find(".container-xl"));
    }

    [Fact]
    public void TabNavbar_DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabNavbar>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void TabNavbar_AppendsCssClass_WhenCssClassIsSet()
    {
        var cut = Render<TabNavbar>(parameters => parameters
            .Add(p => p.CssClass, "my-navbar"));

        var element = cut.Find("header");
        Assert.Contains("my-navbar", element.GetAttribute("class"));
    }

    #endregion

    #region TabSidebar Tests

    [Fact]
    public void TabSidebar_RendersAsideElement()
    {
        var cut = Render<TabSidebar>();

        Assert.NotNull(cut.Find("aside"));
    }

    [Fact]
    public void TabSidebar_HasRequiredClasses()
    {
        var cut = Render<TabSidebar>();

        var element = cut.Find("aside");
        var classes = element.GetAttribute("class");
        Assert.Contains("navbar", classes);
        Assert.Contains("navbar-vertical", classes);
        Assert.Contains("navbar-expand-lg", classes);
    }

    [Fact]
    public void TabSidebar_AddsDarkClass_WhenDarkIsTrue()
    {
        var cut = Render<TabSidebar>(parameters => parameters
            .Add(p => p.Dark, true));

        var element = cut.Find("aside");
        Assert.Contains("navbar-dark", element.GetAttribute("class"));
    }

    [Fact]
    public void TabSidebar_DoesNotAddDarkClass_WhenDarkIsFalse()
    {
        var cut = Render<TabSidebar>(parameters => parameters
            .Add(p => p.Dark, false));

        var element = cut.Find("aside");
        Assert.DoesNotContain("navbar-dark", element.GetAttribute("class"));
    }

    [Fact]
    public void TabSidebar_AddsCondensedClass_WhenCondensedIsTrue()
    {
        var cut = Render<TabSidebar>(parameters => parameters
            .Add(p => p.Condensed, true));

        var element = cut.Find("aside");
        Assert.Contains("navbar-vertical-sm", element.GetAttribute("class"));
    }

    [Fact]
    public void TabSidebar_DoesNotAddCondensedClass_WhenCondensedIsFalse()
    {
        var cut = Render<TabSidebar>(parameters => parameters
            .Add(p => p.Condensed, false));

        var element = cut.Find("aside");
        Assert.DoesNotContain("navbar-vertical-sm", element.GetAttribute("class"));
    }

    [Fact]
    public void TabSidebar_RendersBrand_WhenBrandIsProvided()
    {
        var cut = Render<TabSidebar>(parameters => parameters
            .Add(p => p.Brand, builder =>
            {
                builder.AddMarkupContent(0, "<a href=\"/\">My Brand</a>");
            }));

        Assert.NotNull(cut.Find(".navbar-brand"));
        Assert.Contains("My Brand", cut.Markup);
    }

    [Fact]
    public void TabSidebar_DoesNotRenderBrand_WhenBrandIsNull()
    {
        var cut = Render<TabSidebar>();

        Assert.Throws<Bunit.ElementNotFoundException>(() => cut.Find(".navbar-brand"));
    }

    [Fact]
    public void TabSidebar_RendersNavContent_WhenNavContentIsProvided()
    {
        var cut = Render<TabSidebar>(parameters => parameters
            .Add(p => p.NavContent, builder =>
            {
                builder.AddMarkupContent(0, "<ul class=\"navbar-nav pt-lg-3\"><li>Item</li></ul>");
            }));

        Assert.NotNull(cut.Find(".navbar-collapse"));
        Assert.Contains("Item", cut.Markup);
    }

    [Fact]
    public void TabSidebar_DoesNotRenderNavContent_WhenNavContentIsNull()
    {
        var cut = Render<TabSidebar>();

        Assert.Throws<Bunit.ElementNotFoundException>(() => cut.Find(".navbar-collapse"));
    }

    [Fact]
    public void TabSidebar_DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabSidebar>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void TabSidebar_AppendsCssClass_WhenCssClassIsSet()
    {
        var cut = Render<TabSidebar>(parameters => parameters
            .Add(p => p.CssClass, "my-sidebar"));

        var element = cut.Find("aside");
        Assert.Contains("my-sidebar", element.GetAttribute("class"));
    }

    #endregion

    #region TabPageWrapper Tests

    [Fact]
    public void TabPageWrapper_RendersPageWrapperDiv()
    {
        var cut = Render<TabPageWrapper>();

        var element = cut.Find("div");
        Assert.Contains("page-wrapper", element.GetAttribute("class"));
    }

    [Fact]
    public void TabPageWrapper_RendersChildContent()
    {
        var cut = Render<TabPageWrapper>(parameters => parameters
            .AddChildContent("<p>Content</p>"));

        cut.Find("div").MarkupMatches("<div diff:ignoreAttributes><p>Content</p></div>");
    }

    [Fact]
    public void TabPageWrapper_DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabPageWrapper>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void TabPageWrapper_AppendsCssClass_WhenCssClassIsSet()
    {
        var cut = Render<TabPageWrapper>(parameters => parameters
            .Add(p => p.CssClass, "my-wrapper"));

        var element = cut.Find("div");
        var classes = element.GetAttribute("class");
        Assert.Contains("page-wrapper", classes);
        Assert.Contains("my-wrapper", classes);
    }

    [Fact]
    public void TabPageWrapper_GeneratesUniqueId_WhenNoIdProvided()
    {
        var cut = Render<TabPageWrapper>();

        var element = cut.Find("div");
        var id = element.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("tab-", id);
    }

    #endregion

    #region TabPageBody Tests

    [Fact]
    public void TabPageBody_RendersPageBodyDiv()
    {
        var cut = Render<TabPageBody>();

        var element = cut.Find("div");
        Assert.Contains("page-body", element.GetAttribute("class"));
    }

    [Fact]
    public void TabPageBody_RendersChildContentWithoutContainer_WhenContainerSizeIsNull()
    {
        var cut = Render<TabPageBody>(parameters => parameters
            .AddChildContent("<p>Inner content</p>"));

        var pageBody = cut.Find(".page-body");
        Assert.Contains("Inner content", pageBody.InnerHtml);
        Assert.DoesNotContain("container-", pageBody.InnerHtml);
    }

    [Fact]
    public void TabPageBody_WrapsChildContentInContainerXl_WhenContainerSizeIsExtraLarge()
    {
        var cut = Render<TabPageBody>(parameters => parameters
            .Add(p => p.ContainerSize, ContainerSize.ExtraLarge)
            .AddChildContent("<p>Inner content</p>"));

        Assert.NotNull(cut.Find(".container-xl"));
        Assert.Contains("Inner content", cut.Markup);
    }

    [Fact]
    public void TabPageBody_WrapsChildContentInContainerFluid_WhenContainerSizeIsFluid()
    {
        var cut = Render<TabPageBody>(parameters => parameters
            .Add(p => p.ContainerSize, ContainerSize.Fluid)
            .AddChildContent("<p>Inner content</p>"));

        Assert.NotNull(cut.Find(".container-fluid"));
        Assert.Contains("Inner content", cut.Markup);
    }

    [Fact]
    public void TabPageBody_WrapsChildContentInContainerLg_WhenContainerSizeIsLarge()
    {
        var cut = Render<TabPageBody>(parameters => parameters
            .Add(p => p.ContainerSize, ContainerSize.Large)
            .AddChildContent("<p>Inner content</p>"));

        Assert.NotNull(cut.Find(".container-lg"));
    }

    [Fact]
    public void TabPageBody_DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabPageBody>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void TabPageBody_AppendsCssClass_WhenCssClassIsSet()
    {
        var cut = Render<TabPageBody>(parameters => parameters
            .Add(p => p.CssClass, "my-body"));

        var element = cut.Find("div");
        var classes = element.GetAttribute("class");
        Assert.Contains("page-body", classes);
        Assert.Contains("my-body", classes);
    }

    #endregion

    #region TabFooter Tests

    [Fact]
    public void TabFooter_RendersFooterElement()
    {
        var cut = Render<TabFooter>();

        Assert.NotNull(cut.Find("footer"));
    }

    [Fact]
    public void TabFooter_HasRequiredClasses()
    {
        var cut = Render<TabFooter>();

        var element = cut.Find("footer");
        var classes = element.GetAttribute("class");
        Assert.Contains("footer", classes);
        Assert.Contains("d-print-none", classes);
    }

    [Fact]
    public void TabFooter_AddsTransparentClass_WhenTransparentIsTrue()
    {
        var cut = Render<TabFooter>(parameters => parameters
            .Add(p => p.Transparent, true));

        var element = cut.Find("footer");
        Assert.Contains("footer-transparent", element.GetAttribute("class"));
    }

    [Fact]
    public void TabFooter_DoesNotAddTransparentClass_WhenTransparentIsFalse()
    {
        var cut = Render<TabFooter>(parameters => parameters
            .Add(p => p.Transparent, false));

        var element = cut.Find("footer");
        Assert.DoesNotContain("footer-transparent", element.GetAttribute("class"));
    }

    [Fact]
    public void TabFooter_UsesContainerXl_ByDefault()
    {
        var cut = Render<TabFooter>();

        Assert.NotNull(cut.Find(".container-xl"));
    }

    [Fact]
    public void TabFooter_UsesContainerFluid_WhenContainerSizeIsFluid()
    {
        var cut = Render<TabFooter>(parameters => parameters
            .Add(p => p.ContainerSize, ContainerSize.Fluid));

        Assert.NotNull(cut.Find(".container-fluid"));
    }

    [Fact]
    public void TabFooter_UsesContainerLg_WhenContainerSizeIsLarge()
    {
        var cut = Render<TabFooter>(parameters => parameters
            .Add(p => p.ContainerSize, ContainerSize.Large));

        Assert.NotNull(cut.Find(".container-lg"));
    }

    [Fact]
    public void TabFooter_RendersChildContent_InsideContainerDiv()
    {
        var cut = Render<TabFooter>(parameters => parameters
            .AddChildContent("<p>Footer text</p>"));

        Assert.Contains("Footer text", cut.Find(".container-xl").InnerHtml);
    }

    [Fact]
    public void TabFooter_DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabFooter>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void TabFooter_AppendsCssClass_WhenCssClassIsSet()
    {
        var cut = Render<TabFooter>(parameters => parameters
            .Add(p => p.CssClass, "my-footer"));

        var element = cut.Find("footer");
        var classes = element.GetAttribute("class");
        Assert.Contains("footer", classes);
        Assert.Contains("my-footer", classes);
    }

    [Fact]
    public void TabFooter_GeneratesUniqueId_WhenNoIdProvided()
    {
        var cut = Render<TabFooter>();

        var element = cut.Find("footer");
        var id = element.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("tab-", id);
    }

    #endregion
}
