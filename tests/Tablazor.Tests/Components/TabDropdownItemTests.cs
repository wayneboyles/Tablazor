using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Tablazor.Components;
using Tablazor.Enums;

namespace Tablazor.Components;

public class TabDropdownItemTests : BunitContext
{
    public TabDropdownItemTests()
    {
        Services.AddLogging();
    }

    [Fact]
    public void RendersAsButton_ByDefault()
    {
        var cut = Render<TabDropdownItem>(parameters => parameters
            .AddChildContent("Action"));

        var button = cut.Find("button");
        Assert.NotNull(button);
        Assert.Equal("button", button.GetAttribute("type"));
    }

    [Fact]
    public void RendersAsAnchor_WhenHrefProvided()
    {
        var cut = Render<TabDropdownItem>(parameters => parameters
            .Add(p => p.Href, "https://example.com")
            .AddChildContent("Link"));

        var anchor = cut.Find("a");
        Assert.NotNull(anchor);
        Assert.Equal("https://example.com", anchor.GetAttribute("href"));
    }

    [Fact]
    public void HasDropdownItemClass_ByDefault()
    {
        var cut = Render<TabDropdownItem>(parameters => parameters
            .AddChildContent("Action"));

        var button = cut.Find("button");
        Assert.Contains("dropdown-item", button.GetAttribute("class"));
    }

    [Fact]
    public void AppliesActiveClass_WhenActiveIsTrue()
    {
        var cut = Render<TabDropdownItem>(parameters => parameters
            .Add(p => p.Active, true)
            .AddChildContent("Active Item"));

        var button = cut.Find("button");
        Assert.Contains("active", button.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyActiveClass_WhenActiveIsFalse()
    {
        var cut = Render<TabDropdownItem>(parameters => parameters
            .Add(p => p.Active, false)
            .AddChildContent("Item"));

        var button = cut.Find("button");
        Assert.DoesNotContain("active", button.GetAttribute("class"));
    }

    [Fact]
    public void AppliesDisabledClass_WhenDisabledIsTrue()
    {
        var cut = Render<TabDropdownItem>(parameters => parameters
            .Add(p => p.Disabled, true)
            .AddChildContent("Disabled Item"));

        var button = cut.Find("button");
        Assert.Contains("disabled", button.GetAttribute("class"));
        Assert.True(button.HasAttribute("disabled"));
    }

    [Fact]
    public void AppliesTargetAttribute_WhenHrefAndTargetAreSet()
    {
        var cut = Render<TabDropdownItem>(parameters => parameters
            .Add(p => p.Href, "https://example.com")
            .Add(p => p.Target, "_blank")
            .AddChildContent("External Link"));

        var anchor = cut.Find("a");
        Assert.Equal("_blank", anchor.GetAttribute("target"));
    }

    [Fact]
    public void DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabDropdownItem>(parameters => parameters
            .Add(p => p.Visible, false)
            .AddChildContent("Hidden"));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void RendersChildContent()
    {
        var cut = Render<TabDropdownItem>(parameters => parameters
            .AddChildContent("<strong>Bold Action</strong>"));

        cut.Find("button").MarkupMatches("<button diff:ignoreAttributes><strong>Bold Action</strong></button>");
    }

    [Fact]
    public async Task InvokesOnClick_WhenClicked()
    {
        var clicked = false;
        var cut = Render<TabDropdownItem>(parameters => parameters
            .Add(p => p.OnClick, _ => { clicked = true; })
            .AddChildContent("Click Me"));

        await cut.Find("button").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        Assert.True(clicked);
    }

    [Fact]
    public async Task DoesNotInvokeOnClick_WhenDisabled()
    {
        var clicked = false;
        var cut = Render<TabDropdownItem>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.OnClick, _ => { clicked = true; })
            .AddChildContent("Disabled"));

        await cut.Find("button").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        Assert.False(clicked);
    }

    [Fact]
    public void AppliesCustomCssClass()
    {
        var cut = Render<TabDropdownItem>(parameters => parameters
            .Add(p => p.CssClass, "custom-item-class")
            .AddChildContent("Item"));

        var button = cut.Find("button");
        Assert.Contains("custom-item-class", button.GetAttribute("class"));
    }

    [Fact]
    public void AppliesStyle_WhenStyleIsSet()
    {
        var cut = Render<TabDropdownItem>(parameters => parameters
            .Add(p => p.Style, "color: red;")
            .AddChildContent("Styled Item"));

        var button = cut.Find("button");
        Assert.Equal("color: red;", button.GetAttribute("style"));
    }

    [Fact]
    public void AppliesAdditionalAttributes()
    {
        var cut = Render<TabDropdownItem>(parameters => parameters
            .AddUnmatched("data-testid", "my-item")
            .AddUnmatched("aria-label", "Item label")
            .AddChildContent("Item"));

        var button = cut.Find("button");
        Assert.Equal("my-item", button.GetAttribute("data-testid"));
        Assert.Equal("Item label", button.GetAttribute("aria-label"));
    }

    [Fact]
    public void GeneratesUniqueId_WhenNoIdProvided()
    {
        var cut = Render<TabDropdownItem>(parameters => parameters
            .AddChildContent("Item"));

        var button = cut.Find("button");
        var id = button.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("tab-", id);
    }

    [Fact]
    public void UsesProvidedId_WhenIdIsSet()
    {
        var cut = Render<TabDropdownItem>(parameters => parameters
            .AddUnmatched("id", "custom-item-id")
            .AddChildContent("Item"));

        var button = cut.Find("button");
        Assert.Equal("custom-item-id", button.GetAttribute("id"));
    }

    [Fact]
    public void HasDropdownItemClass_OnAnchor_WhenHrefProvided()
    {
        var cut = Render<TabDropdownItem>(parameters => parameters
            .Add(p => p.Href, "#")
            .AddChildContent("Link Item"));

        var anchor = cut.Find("a");
        Assert.Contains("dropdown-item", anchor.GetAttribute("class"));
    }

    [Fact]
    public void AppliesActiveClass_OnAnchor_WhenActiveIsTrue()
    {
        var cut = Render<TabDropdownItem>(parameters => parameters
            .Add(p => p.Href, "#")
            .Add(p => p.Active, true)
            .AddChildContent("Active Link"));

        var anchor = cut.Find("a");
        Assert.Contains("active", anchor.GetAttribute("class"));
    }

    [Fact]
    public void AppliesDisabledClass_OnAnchor_WhenDisabledIsTrue()
    {
        var cut = Render<TabDropdownItem>(parameters => parameters
            .Add(p => p.Href, "#")
            .Add(p => p.Disabled, true)
            .AddChildContent("Disabled Link"));

        var anchor = cut.Find("a");
        Assert.Contains("disabled", anchor.GetAttribute("class"));
    }
}
