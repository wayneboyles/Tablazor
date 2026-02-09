using System.Threading.Tasks;
using Tablazor.Components;
using Tablazor.Enums;

namespace Tablazor.Components;

public class TabButtonTests : BunitContext
{
    public TabButtonTests()
    {
        Services.AddLogging();
    }

    [Fact]
    public void RendersAsButton_ByDefault()
    {
        var cut = Render<TabButton>(parameters => parameters
            .AddChildContent("Click Me"));

        var button = cut.Find("button");
        Assert.NotNull(button);
        Assert.Equal("button", button.GetAttribute("type"));
    }

    [Fact]
    public void RendersAsSubmitButton_WhenTypeIsSubmit()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Type, ButtonType.Submit)
            .AddChildContent("Submit"));

        var button = cut.Find("button");
        Assert.Equal("submit", button.GetAttribute("type"));
    }

    [Fact]
    public void RendersAsResetButton_WhenTypeIsReset()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Type, ButtonType.Reset)
            .AddChildContent("Reset"));

        var button = cut.Find("button");
        Assert.Equal("reset", button.GetAttribute("type"));
    }

    [Fact]
    public void RendersAsAnchor_WhenTypeIsLink()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Type, ButtonType.Link)
            .Add(p => p.Href, "https://example.com")
            .AddChildContent("Link"));

        var anchor = cut.Find("a");
        Assert.NotNull(anchor);
        Assert.Equal("https://example.com", anchor.GetAttribute("href"));
        Assert.Equal("button", anchor.GetAttribute("role"));
    }

    [Fact]
    public void HasBtnClass_ByDefault()
    {
        var cut = Render<TabButton>();

        var button = cut.Find("button");
        Assert.Contains("btn", button.GetAttribute("class"));
    }

    [Fact]
    public void AppliesColorClass_WhenColorIsSet()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Color, TabColors.Primary));

        var button = cut.Find("button");
        Assert.Contains("btn-primary", button.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyColorClass_WhenColorIsDefault()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Color, TabColors.Default));

        var button = cut.Find("button");
        var classes = button.GetAttribute("class");
        Assert.DoesNotContain("btn-primary", classes);
        Assert.DoesNotContain("btn-secondary", classes);
    }

    [Fact]
    public void AppliesOutlineClass_WhenOutlineIsTrue()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Color, TabColors.Primary)
            .Add(p => p.Outline, true));

        var button = cut.Find("button");
        Assert.Contains("btn-outline-primary", button.GetAttribute("class"));
    }

    [Fact]
    public void AppliesGhostClass_WhenGhostIsTrue()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Color, TabColors.Primary)
            .Add(p => p.Ghost, true));

        var button = cut.Find("button");
        Assert.Contains("btn-ghost-primary", button.GetAttribute("class"));
    }

    [Fact]
    public void AppliesSizeClass_WhenSizeIsSmall()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Size, ButtonSize.Small));

        var button = cut.Find("button");
        Assert.Contains("btn-sm", button.GetAttribute("class"));
    }

    [Fact]
    public void AppliesSizeClass_WhenSizeIsLarge()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Size, ButtonSize.Large));

        var button = cut.Find("button");
        Assert.Contains("btn-lg", button.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplySizeClass_WhenSizeIsDefault()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Size, ButtonSize.Default));

        var button = cut.Find("button");
        var classes = button.GetAttribute("class");
        Assert.DoesNotContain("btn-sm", classes);
        Assert.DoesNotContain("btn-lg", classes);
    }

    [Fact]
    public void AppliesIconOnlyClass_WhenIconOnlyIsTrue()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.IconOnly, true));

        var button = cut.Find("button");
        Assert.Contains("btn-icon", button.GetAttribute("class"));
    }

    [Fact]
    public void AppliesSquareClass_WhenSquareIsTrue()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Square, true));

        var button = cut.Find("button");
        Assert.Contains("btn-square", button.GetAttribute("class"));
    }

    [Fact]
    public void AppliesPillClass_WhenPillIsTrue()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Pill, true));

        var button = cut.Find("button");
        Assert.Contains("btn-pill", button.GetAttribute("class"));
    }

    [Fact]
    public void AppliesBlockClass_WhenBlockIsTrue()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Block, true));

        var button = cut.Find("button");
        Assert.Contains("w-100", button.GetAttribute("class"));
    }

    [Fact]
    public void AppliesDisabledAttribute_WhenDisabledIsTrue()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Disabled, true));

        var button = cut.Find("button");
        Assert.True(button.HasAttribute("disabled"));
    }

    [Fact]
    public void AppliesDisabledClassToLink_WhenDisabledIsTrue()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Type, ButtonType.Link)
            .Add(p => p.Href, "#")
            .Add(p => p.Disabled, true));

        var anchor = cut.Find("a");
        Assert.Contains("disabled", anchor.GetAttribute("class"));
        Assert.Equal("true", anchor.GetAttribute("aria-disabled"));
        Assert.Equal("-1", anchor.GetAttribute("tabindex"));
    }

    [Fact]
    public void ShowsSpinner_WhenIsLoadingIsTrue()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.IsLoading, true)
            .AddChildContent("Loading"));

        var spinner = cut.Find(".spinner-border");
        Assert.NotNull(spinner);
    }

    [Fact]
    public void DoesNotShowSpinner_WhenIsLoadingIsFalse()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.IsLoading, false)
            .AddChildContent("Not Loading"));

        Assert.Throws<Bunit.ElementNotFoundException>(() => cut.Find(".spinner-border"));
    }

    [Fact]
    public void RendersChildContent()
    {
        var cut = Render<TabButton>(parameters => parameters
            .AddChildContent("<strong>Bold Text</strong>"));

        cut.Find("button").MarkupMatches("<button diff:ignoreAttributes><strong>Bold Text</strong></button>");
    }

    [Fact]
    public void DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Visible, false)
            .AddChildContent("Hidden"));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void AppliesStyle_WhenStyleIsSet()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Style, "color: red;"));

        var button = cut.Find("button");
        Assert.Equal("color: red;", button.GetAttribute("style"));
    }

    [Fact]
    public void AppliesAdditionalAttributes()
    {
        var cut = Render<TabButton>(parameters => parameters
            .AddUnmatched("data-testid", "my-button")
            .AddUnmatched("aria-label", "Button label"));

        var button = cut.Find("button");
        Assert.Equal("my-button", button.GetAttribute("data-testid"));
        Assert.Equal("Button label", button.GetAttribute("aria-label"));
    }

    [Fact]
    public void GeneratesUniqueId_WhenNoIdProvided()
    {
        var cut = Render<TabButton>();

        var button = cut.Find("button");
        var id = button.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("tab-", id);
    }

    [Fact]
    public void UsesProvidedId_WhenIdIsSet()
    {
        var cut = Render<TabButton>(parameters => parameters
            .AddUnmatched("id", "custom-button-id"));

        var button = cut.Find("button");
        Assert.Equal("custom-button-id", button.GetAttribute("id"));
    }

    [Fact]
    public async Task InvokesOnClick_WhenButtonIsClicked()
    {
        var clicked = false;
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.OnClick, _ => { clicked = true; }));

        await cut.Find("button").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        Assert.True(clicked);
    }

    [Fact]
    public async Task DoesNotInvokeOnClick_WhenDisabled()
    {
        var clicked = false;
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.OnClick, _ => { clicked = true; }));

        await cut.Find("button").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        Assert.False(clicked);
    }

    [Fact]
    public async Task DoesNotInvokeOnClick_WhenIsLoading()
    {
        var clicked = false;
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.IsLoading, true)
            .Add(p => p.OnClick, _ => { clicked = true; }));

        await cut.Find("button").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        Assert.False(clicked);
    }

    [Fact]
    public void AppliesCssClass_WhenCssClassIsSet()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.CssClass, "custom-class"));

        var button = cut.Find("button");
        Assert.Contains("custom-class", button.GetAttribute("class"));
    }

    [Fact]
    public void CombinesMultipleClasses()
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Color, TabColors.Primary)
            .Add(p => p.Size, ButtonSize.Large)
            .Add(p => p.Pill, true)
            .Add(p => p.Outline, true));

        var button = cut.Find("button");
        var classes = button.GetAttribute("class");
        Assert.Contains("btn", classes);
        Assert.Contains("btn-outline-primary", classes);
        Assert.Contains("btn-lg", classes);
        Assert.Contains("btn-pill", classes);
    }

    [Theory]
    [InlineData(TabColors.Primary, "btn-primary")]
    [InlineData(TabColors.Secondary, "btn-secondary")]
    [InlineData(TabColors.Success, "btn-success")]
    [InlineData(TabColors.Danger, "btn-danger")]
    [InlineData(TabColors.Warning, "btn-warning")]
    [InlineData(TabColors.Info, "btn-info")]
    public void AppliesCorrectColorClass_ForEachColor(TabColors color, string expectedClass)
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Color, color));

        var button = cut.Find("button");
        Assert.Contains(expectedClass, button.GetAttribute("class"));
    }

    [Theory]
    [InlineData(ButtonSize.Small, "btn-sm")]
    [InlineData(ButtonSize.Large, "btn-lg")]
    [InlineData(ButtonSize.ExtraLarge, "btn-xl")]
    public void AppliesCorrectSizeClass_ForEachSize(ButtonSize size, string expectedClass)
    {
        var cut = Render<TabButton>(parameters => parameters
            .Add(p => p.Size, size));

        var button = cut.Find("button");
        Assert.Contains(expectedClass, button.GetAttribute("class"));
    }
}
