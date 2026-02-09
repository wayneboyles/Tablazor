using Tablazor.Components;
using Tablazor.Enums;
using Tablazor.Services;

namespace Tablazor.Components;

public class TabToastContainerTests : BunitContext
{
    public TabToastContainerTests()
    {
        Services.AddLogging();
        Services.AddScoped<TabToastService>();
    }

    [Fact]
    public void DoesNotRender_WhenNoToasts()
    {
        var cut = Render<TabToastContainer>();

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void RendersContainer_WhenToastsExist()
    {
        var toastService = Services.GetRequiredService<TabToastService>();
        toastService.Show("Test message");

        var cut = Render<TabToastContainer>();

        var container = cut.Find(".toast-container");
        Assert.NotNull(container);
    }

    [Fact]
    public void RendersToasts_FromService()
    {
        var toastService = Services.GetRequiredService<TabToastService>();
        toastService.Show("Message 1");
        toastService.Show("Message 2");

        var cut = Render<TabToastContainer>();

        var toasts = cut.FindAll(".toast");
        Assert.Equal(2, toasts.Count);
    }

    [Fact]
    public void HasPositionFixed_ByDefault()
    {
        var toastService = Services.GetRequiredService<TabToastService>();
        toastService.Show("Test");

        var cut = Render<TabToastContainer>();

        var container = cut.Find(".toast-container");
        Assert.Contains("position-fixed", container.GetAttribute("class"));
    }

    [Fact]
    public void AppliesTopRightPosition_ByDefault()
    {
        var toastService = Services.GetRequiredService<TabToastService>();
        toastService.Show("Test");

        var cut = Render<TabToastContainer>();

        var container = cut.Find(".toast-container");
        var classes = container.GetAttribute("class");
        Assert.Contains("top-0", classes);
        Assert.Contains("end-0", classes);
    }

    [Fact]
    public void AppliesBottomLeftPosition_WhenSet()
    {
        var toastService = Services.GetRequiredService<TabToastService>();
        toastService.Show("Test");

        var cut = Render<TabToastContainer>(parameters => parameters
            .Add(p => p.Position, ToastPosition.BottomLeft));

        var container = cut.Find(".toast-container");
        var classes = container.GetAttribute("class");
        Assert.Contains("bottom-0", classes);
        Assert.Contains("start-0", classes);
    }

    [Fact]
    public void AppliesTopCenterPosition_WhenSet()
    {
        var toastService = Services.GetRequiredService<TabToastService>();
        toastService.Show("Test");

        var cut = Render<TabToastContainer>(parameters => parameters
            .Add(p => p.Position, ToastPosition.TopCenter));

        var container = cut.Find(".toast-container");
        var classes = container.GetAttribute("class");
        Assert.Contains("top-0", classes);
        Assert.Contains("start-50", classes);
        Assert.Contains("translate-middle-x", classes);
    }

    [Fact]
    public void AppliesZIndex_InStyle()
    {
        var toastService = Services.GetRequiredService<TabToastService>();
        toastService.Show("Test");

        var cut = Render<TabToastContainer>(parameters => parameters
            .Add(p => p.ZIndex, 9999));

        var container = cut.Find(".toast-container");
        var style = container.GetAttribute("style");
        Assert.Contains("z-index: 9999", style);
    }

    [Fact]
    public void HasDefaultZIndex_Of1090()
    {
        var toastService = Services.GetRequiredService<TabToastService>();
        toastService.Show("Test");

        var cut = Render<TabToastContainer>();

        var container = cut.Find(".toast-container");
        var style = container.GetAttribute("style");
        Assert.Contains("z-index: 1090", style);
    }

    [Fact]
    public void HasAriaAttributes_ForAccessibility()
    {
        var toastService = Services.GetRequiredService<TabToastService>();
        toastService.Show("Test");

        var cut = Render<TabToastContainer>();

        var container = cut.Find(".toast-container");
        Assert.Equal("polite", container.GetAttribute("aria-live"));
        Assert.Equal("true", container.GetAttribute("aria-atomic"));
    }

    [Fact]
    public void UpdatesWhenToastAdded()
    {
        var toastService = Services.GetRequiredService<TabToastService>();
        var cut = Render<TabToastContainer>();

        Assert.Empty(cut.Markup);

        toastService.Show("New toast");
        cut.WaitForState(() => cut.FindAll(".toast").Count == 1);

        var toasts = cut.FindAll(".toast");
        Assert.Single(toasts);
    }

    [Fact]
    public void DoesNotRender_WhenVisibleIsFalse()
    {
        var toastService = Services.GetRequiredService<TabToastService>();
        toastService.Show("Test");

        var cut = Render<TabToastContainer>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void HasPaddingClass()
    {
        var toastService = Services.GetRequiredService<TabToastService>();
        toastService.Show("Test");

        var cut = Render<TabToastContainer>();

        var container = cut.Find(".toast-container");
        Assert.Contains("p-3", container.GetAttribute("class"));
    }
}
