using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Tablazor.Components;
using Tablazor.Enums;

namespace Tablazor.Components;

public class TabDropdownMenuTests : BunitContext
{
    public TabDropdownMenuTests()
    {
        Services.AddLogging();
    }

    [Fact]
    public void RendersMenu_WhenDropdownIsOpen()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabDropdownMenu>(0);
                builder.AddAttribute(1, "ChildContent",
                    (RenderFragment)(b => b.AddContent(0, "Menu Content")));
                builder.CloseComponent();
            }));

        var menu = cut.Find("div.dropdown-menu");
        Assert.NotNull(menu);
    }

    [Fact]
    public void DoesNotHaveShowClass_WhenDropdownIsClosed()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.IsOpen, false)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabDropdownMenu>(0);
                builder.AddAttribute(1, "ChildContent",
                    (RenderFragment)(b => b.AddContent(0, "Menu Content")));
                builder.CloseComponent();
            }));

        var menu = cut.Find("div.dropdown-menu");
        Assert.DoesNotContain("show", menu.GetAttribute("class"));
    }

    [Fact]
    public void HasShowClass_WhenDropdownIsOpen()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabDropdownMenu>(0);
                builder.AddAttribute(1, "ChildContent",
                    (RenderFragment)(b => b.AddContent(0, "Menu Content")));
                builder.CloseComponent();
            }));

        var menu = cut.Find("div.dropdown-menu");
        Assert.Contains("show", menu.GetAttribute("class"));
    }

    [Fact]
    public void AppliesEndAlignmentClass()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabDropdownMenu>(0);
                builder.AddAttribute(1, "Alignment", DropdownAlignment.End);
                builder.AddAttribute(2, "ChildContent",
                    (RenderFragment)(b => b.AddContent(0, "Menu Content")));
                builder.CloseComponent();
            }));

        var menu = cut.Find("div.dropdown-menu");
        Assert.Contains("dropdown-menu-end", menu.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyAlignmentClass_WhenAlignmentIsStart()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabDropdownMenu>(0);
                builder.AddAttribute(1, "Alignment", DropdownAlignment.Start);
                builder.AddAttribute(2, "ChildContent",
                    (RenderFragment)(b => b.AddContent(0, "Menu Content")));
                builder.CloseComponent();
            }));

        var menu = cut.Find("div.dropdown-menu");
        Assert.DoesNotContain("dropdown-menu-start", menu.GetAttribute("class"));
        Assert.DoesNotContain("dropdown-menu-end", menu.GetAttribute("class"));
    }

    [Fact]
    public void RendersChildContent()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabDropdownMenu>(0);
                builder.AddAttribute(1, "ChildContent",
                    (RenderFragment)(b =>
                    {
                        b.OpenElement(0, "span");
                        b.AddContent(1, "Item Text");
                        b.CloseElement();
                    }));
                builder.CloseComponent();
            }));

        var menu = cut.Find("div.dropdown-menu");
        Assert.Contains("Item Text", menu.InnerHtml);
    }

    [Fact]
    public void AppliesCustomCssClass()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabDropdownMenu>(0);
                builder.AddAttribute(1, "CssClass", "custom-menu-class");
                builder.AddAttribute(2, "ChildContent",
                    (RenderFragment)(b => b.AddContent(0, "Content")));
                builder.CloseComponent();
            }));

        var menu = cut.Find("div.dropdown-menu");
        Assert.Contains("custom-menu-class", menu.GetAttribute("class"));
    }

    [Fact]
    public async Task MenuShowClassAppearsAndDisappears_WhenToggled()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TabDropdownMenu>(0);
                builder.AddAttribute(1, "ChildContent",
                    (RenderFragment)(b => b.AddContent(0, "Menu Content")));
                builder.CloseComponent();
            }));

        // Menu should not have show class initially
        var menu = cut.Find("div.dropdown-menu");
        Assert.DoesNotContain("show", menu.GetAttribute("class"));

        // Open
        await cut.InvokeAsync(() => cut.Instance.ToggleAsync());

        menu = cut.Find("div.dropdown-menu");
        Assert.Contains("show", menu.GetAttribute("class"));

        // Close
        await cut.InvokeAsync(() => cut.Instance.ToggleAsync());

        menu = cut.Find("div.dropdown-menu");
        Assert.DoesNotContain("show", menu.GetAttribute("class"));
    }
}
