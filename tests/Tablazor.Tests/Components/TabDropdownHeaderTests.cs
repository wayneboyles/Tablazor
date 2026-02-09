using Tablazor.Components;

namespace Tablazor.Components;

public class TabDropdownHeaderTests : BunitContext
{
    public TabDropdownHeaderTests()
    {
        Services.AddLogging();
    }

    [Fact]
    public void RendersAsH6Element()
    {
        var cut = Render<TabDropdownHeader>(parameters => parameters
            .AddChildContent("Header"));

        var header = cut.Find("h6");
        Assert.NotNull(header);
    }

    [Fact]
    public void HasDropdownHeaderClass()
    {
        var cut = Render<TabDropdownHeader>(parameters => parameters
            .AddChildContent("Header"));

        var header = cut.Find("h6");
        Assert.Contains("dropdown-header", header.GetAttribute("class"));
    }

    [Fact]
    public void RendersChildContent()
    {
        var cut = Render<TabDropdownHeader>(parameters => parameters
            .AddChildContent("Section Title"));

        var header = cut.Find("h6");
        Assert.Contains("Section Title", header.TextContent);
    }

    [Fact]
    public void RendersHtmlChildContent()
    {
        var cut = Render<TabDropdownHeader>(parameters => parameters
            .AddChildContent("<strong>Bold Header</strong>"));

        cut.Find("h6").MarkupMatches("<h6 diff:ignoreAttributes><strong>Bold Header</strong></h6>");
    }

    [Fact]
    public void DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabDropdownHeader>(parameters => parameters
            .Add(p => p.Visible, false)
            .AddChildContent("Hidden"));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void GeneratesUniqueId_WhenNoIdProvided()
    {
        var cut = Render<TabDropdownHeader>(parameters => parameters
            .AddChildContent("Header"));

        var header = cut.Find("h6");
        var id = header.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("tab-", id);
    }

    [Fact]
    public void UsesProvidedId_WhenIdIsSet()
    {
        var cut = Render<TabDropdownHeader>(parameters => parameters
            .AddUnmatched("id", "custom-header-id")
            .AddChildContent("Header"));

        var header = cut.Find("h6");
        Assert.Equal("custom-header-id", header.GetAttribute("id"));
    }

    [Fact]
    public void AppliesCustomCssClass()
    {
        var cut = Render<TabDropdownHeader>(parameters => parameters
            .Add(p => p.CssClass, "custom-header-class")
            .AddChildContent("Header"));

        var header = cut.Find("h6");
        Assert.Contains("custom-header-class", header.GetAttribute("class"));
    }

    [Fact]
    public void AppliesStyle_WhenStyleIsSet()
    {
        var cut = Render<TabDropdownHeader>(parameters => parameters
            .Add(p => p.Style, "font-size: 14px;")
            .AddChildContent("Header"));

        var header = cut.Find("h6");
        Assert.Equal("font-size: 14px;", header.GetAttribute("style"));
    }

    [Fact]
    public void AppliesAdditionalAttributes()
    {
        var cut = Render<TabDropdownHeader>(parameters => parameters
            .AddUnmatched("data-testid", "my-header")
            .AddChildContent("Header"));

        var header = cut.Find("h6");
        Assert.Equal("my-header", header.GetAttribute("data-testid"));
    }
}
