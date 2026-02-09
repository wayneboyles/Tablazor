using Tablazor.Components;

namespace Tablazor.Components;

public class TabDropdownDividerTests : BunitContext
{
    public TabDropdownDividerTests()
    {
        Services.AddLogging();
    }

    [Fact]
    public void RendersDividerElement()
    {
        var cut = Render<TabDropdownDivider>();

        var divider = cut.Find("div");
        Assert.NotNull(divider);
    }

    [Fact]
    public void HasDropdownDividerClass()
    {
        var cut = Render<TabDropdownDivider>();

        var divider = cut.Find("div");
        Assert.Contains("dropdown-divider", divider.GetAttribute("class"));
    }

    [Fact]
    public void HasSeparatorRole()
    {
        var cut = Render<TabDropdownDivider>();

        var divider = cut.Find("div");
        Assert.Equal("separator", divider.GetAttribute("role"));
    }

    [Fact]
    public void DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabDropdownDivider>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void GeneratesUniqueId_WhenNoIdProvided()
    {
        var cut = Render<TabDropdownDivider>();

        var divider = cut.Find("div");
        var id = divider.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("tab-", id);
    }

    [Fact]
    public void UsesProvidedId_WhenIdIsSet()
    {
        var cut = Render<TabDropdownDivider>(parameters => parameters
            .AddUnmatched("id", "custom-divider-id"));

        var divider = cut.Find("div");
        Assert.Equal("custom-divider-id", divider.GetAttribute("id"));
    }

    [Fact]
    public void AppliesCustomCssClass()
    {
        var cut = Render<TabDropdownDivider>(parameters => parameters
            .Add(p => p.CssClass, "my-divider"));

        var divider = cut.Find("div");
        Assert.Contains("my-divider", divider.GetAttribute("class"));
    }

    [Fact]
    public void AppliesStyle_WhenStyleIsSet()
    {
        var cut = Render<TabDropdownDivider>(parameters => parameters
            .Add(p => p.Style, "margin: 5px 0;"));

        var divider = cut.Find("div");
        Assert.Equal("margin: 5px 0;", divider.GetAttribute("style"));
    }

    [Fact]
    public void AppliesAdditionalAttributes()
    {
        var cut = Render<TabDropdownDivider>(parameters => parameters
            .AddUnmatched("data-testid", "my-divider"));

        var divider = cut.Find("div");
        Assert.Equal("my-divider", divider.GetAttribute("data-testid"));
    }
}
