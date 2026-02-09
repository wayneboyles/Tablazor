using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Tablazor.Components;
using Tablazor.Enums;

namespace Tablazor.Components;

public class TabDropdownTests : BunitContext
{
    public TabDropdownTests()
    {
        Services.AddLogging();
    }

    [Fact]
    public void RendersDropdownElement()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .AddChildContent("Toggle"));

        var dropdown = cut.Find("div");
        Assert.NotNull(dropdown);
    }

    [Fact]
    public void HasDropdownClass_ByDefault()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .AddChildContent("Toggle"));

        var dropdown = cut.Find("div");
        Assert.Contains("dropdown", dropdown.GetAttribute("class"));
    }

    [Fact]
    public void AppliesDropupClass_WhenDirectionIsUp()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.Direction, DropdownDirection.Up)
            .AddChildContent("Toggle"));

        var dropdown = cut.Find("div");
        Assert.Contains("dropup", dropdown.GetAttribute("class"));
    }

    [Fact]
    public void AppliesDropstartClass_WhenDirectionIsStart()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.Direction, DropdownDirection.Start)
            .AddChildContent("Toggle"));

        var dropdown = cut.Find("div");
        Assert.Contains("dropstart", dropdown.GetAttribute("class"));
    }

    [Fact]
    public void AppliesDropendClass_WhenDirectionIsEnd()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.Direction, DropdownDirection.End)
            .AddChildContent("Toggle"));

        var dropdown = cut.Find("div");
        Assert.Contains("dropend", dropdown.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.Visible, false)
            .AddChildContent("Toggle"));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void GeneratesUniqueId_WhenNoIdProvided()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .AddChildContent("Toggle"));

        var dropdown = cut.Find("div");
        var id = dropdown.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("tab-", id);
    }

    [Fact]
    public void UsesProvidedId_WhenIdIsSet()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .AddUnmatched("id", "custom-dropdown-id")
            .AddChildContent("Toggle"));

        var dropdown = cut.Find("div");
        Assert.Equal("custom-dropdown-id", dropdown.GetAttribute("id"));
    }

    [Fact]
    public void AppliesCustomCssClass()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.CssClass, "my-custom-class")
            .AddChildContent("Toggle"));

        var dropdown = cut.Find("div");
        Assert.Contains("my-custom-class", dropdown.GetAttribute("class"));
    }

    [Fact]
    public void AppliesStyle_WhenStyleIsSet()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.Style, "margin-top: 10px;")
            .AddChildContent("Toggle"));

        var dropdown = cut.Find("div");
        Assert.Equal("margin-top: 10px;", dropdown.GetAttribute("style"));
    }

    [Fact]
    public void AppliesAdditionalAttributes()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .AddUnmatched("data-testid", "my-dropdown")
            .AddChildContent("Toggle"));

        var dropdown = cut.Find("div");
        Assert.Equal("my-dropdown", dropdown.GetAttribute("data-testid"));
    }

    [Fact]
    public void IsMenuOpen_IsFalse_ByDefault()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .AddChildContent("Toggle"));

        Assert.False(cut.Instance.IsMenuOpen);
    }

    [Fact]
    public async Task ToggleAsync_OpensDropdown_WhenClosed()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .AddChildContent("Toggle"));

        await cut.InvokeAsync(() => cut.Instance.ToggleAsync());

        Assert.True(cut.Instance.IsMenuOpen);
    }

    [Fact]
    public async Task ToggleAsync_ClosesDropdown_WhenOpen()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .AddChildContent("Toggle"));

        await cut.InvokeAsync(() => cut.Instance.ToggleAsync());

        Assert.False(cut.Instance.IsMenuOpen);
    }

    [Fact]
    public async Task OpenAsync_SetsIsMenuOpen_ToTrue()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .AddChildContent("Toggle"));

        await cut.InvokeAsync(() => cut.Instance.OpenAsync());

        Assert.True(cut.Instance.IsMenuOpen);
    }

    [Fact]
    public async Task OpenAsync_DoesNothing_WhenAlreadyOpen()
    {
        var openChangedCount = 0;
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.IsOpenChanged, EventCallback.Factory.Create<bool>(this, _ => openChangedCount++))
            .AddChildContent("Toggle"));

        await cut.InvokeAsync(() => cut.Instance.OpenAsync());

        Assert.Equal(0, openChangedCount);
    }

    [Fact]
    public async Task CloseAsync_SetsIsMenuOpen_ToFalse()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .AddChildContent("Toggle"));

        await cut.InvokeAsync(() => cut.Instance.CloseAsync());

        Assert.False(cut.Instance.IsMenuOpen);
    }

    [Fact]
    public async Task CloseAsync_DoesNothing_WhenAlreadyClosed()
    {
        var closedCount = 0;
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.OnClosed, EventCallback.Factory.Create(this, () => closedCount++))
            .AddChildContent("Toggle"));

        await cut.InvokeAsync(() => cut.Instance.CloseAsync());

        Assert.Equal(0, closedCount);
    }

    [Fact]
    public async Task IsOpenChanged_IsInvoked_WhenToggled()
    {
        var isOpenValue = false;
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.IsOpenChanged, EventCallback.Factory.Create<bool>(this, value => isOpenValue = value))
            .AddChildContent("Toggle"));

        await cut.InvokeAsync(() => cut.Instance.ToggleAsync());

        Assert.True(isOpenValue);
    }

    [Fact]
    public async Task OnOpened_IsInvoked_WhenDropdownOpens()
    {
        var opened = false;
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.OnOpened, EventCallback.Factory.Create(this, () => opened = true))
            .AddChildContent("Toggle"));

        await cut.InvokeAsync(() => cut.Instance.OpenAsync());

        Assert.True(opened);
    }

    [Fact]
    public async Task OnClosed_IsInvoked_WhenDropdownCloses()
    {
        var closed = false;
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.OnClosed, EventCallback.Factory.Create(this, () => closed = true))
            .AddChildContent("Toggle"));

        await cut.InvokeAsync(() => cut.Instance.CloseAsync());

        Assert.True(closed);
    }

    [Fact]
    public void IsOpen_Parameter_SetsInitialState()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .AddChildContent("Toggle"));

        Assert.True(cut.Instance.IsMenuOpen);
    }

    [Fact]
    public void RendersChildContent()
    {
        var cut = Render<TabDropdown>(parameters => parameters
            .AddChildContent("<span>My Content</span>"));

        cut.Find("div").MarkupMatches("<div diff:ignoreAttributes><span>My Content</span></div>");
    }
}
