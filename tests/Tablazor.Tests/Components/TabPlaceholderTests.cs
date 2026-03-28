using Tablazor.Components;
using Tablazor.Enums;

namespace Tablazor.Components;

public class TabPlaceholderTests : BunitContext
{
    public TabPlaceholderTests()
    {
        Services.AddLogging();
    }

    // -------------------------------------------------------------------------
    // TabPlaceholder
    // -------------------------------------------------------------------------

    [Fact]
    public void RendersSpan_WithPlaceholderClass()
    {
        var cut = Render<TabPlaceholder>();

        var element = cut.Find("span");
        Assert.Contains("placeholder", element.GetAttribute("class"));
    }

    [Fact]
    public void DefaultColumns_AddsCol12Class()
    {
        var cut = Render<TabPlaceholder>();

        Assert.Contains("col-12", cut.Find("span").GetAttribute("class"));
    }

    [Fact]
    public void Columns6_AddsCol6Class()
    {
        var cut = Render<TabPlaceholder>(parameters => parameters
            .Add(p => p.Columns, 6));

        Assert.Contains("col-6", cut.Find("span").GetAttribute("class"));
    }

    [Fact]
    public void Columns0_IsClamped_ToCol1()
    {
        var cut = Render<TabPlaceholder>(parameters => parameters
            .Add(p => p.Columns, 0));

        var classes = cut.Find("span").GetAttribute("class");
        Assert.Contains("col-1", classes);
        Assert.DoesNotContain("col-0", classes);
    }

    [Fact]
    public void Columns13_IsClamped_ToCol12()
    {
        var cut = Render<TabPlaceholder>(parameters => parameters
            .Add(p => p.Columns, 13));

        var classes = cut.Find("span").GetAttribute("class");
        Assert.Contains("col-12", classes);
        Assert.DoesNotContain("col-13", classes);
    }

    [Fact]
    public void SizeLarge_AddsPlaceholderLgClass()
    {
        var cut = Render<TabPlaceholder>(parameters => parameters
            .Add(p => p.Size, PlaceholderSize.Large));

        Assert.Contains("placeholder-lg", cut.Find("span").GetAttribute("class"));
    }

    [Fact]
    public void SizeSmall_AddsPlaceholderSmClass()
    {
        var cut = Render<TabPlaceholder>(parameters => parameters
            .Add(p => p.Size, PlaceholderSize.Small));

        Assert.Contains("placeholder-sm", cut.Find("span").GetAttribute("class"));
    }

    [Fact]
    public void SizeExtraSmall_AddsPlaceholderXsClass()
    {
        var cut = Render<TabPlaceholder>(parameters => parameters
            .Add(p => p.Size, PlaceholderSize.ExtraSmall));

        Assert.Contains("placeholder-xs", cut.Find("span").GetAttribute("class"));
    }

    [Fact]
    public void SizeDefault_DoesNotAddSizeModifierClass()
    {
        var cut = Render<TabPlaceholder>(parameters => parameters
            .Add(p => p.Size, PlaceholderSize.Default));

        var classes = cut.Find("span").GetAttribute("class");
        Assert.DoesNotContain("placeholder-xs", classes);
        Assert.DoesNotContain("placeholder-sm", classes);
        Assert.DoesNotContain("placeholder-lg", classes);
    }

    [Fact]
    public void ColorPrimary_AddsBgPrimaryClass()
    {
        var cut = Render<TabPlaceholder>(parameters => parameters
            .Add(p => p.Color, TabColors.Primary));

        Assert.Contains("bg-primary", cut.Find("span").GetAttribute("class"));
    }

    [Fact]
    public void ColorDefault_DoesNotAddBgClass()
    {
        var cut = Render<TabPlaceholder>(parameters => parameters
            .Add(p => p.Color, TabColors.Default));

        Assert.DoesNotContain("bg-", cut.Find("span").GetAttribute("class"));
    }

    [Fact]
    public void Width_Set_OmitsColClass_AndAddsWidthStyle()
    {
        var cut = Render<TabPlaceholder>(parameters => parameters
            .Add(p => p.Width, "25%"));

        var element = cut.Find("span");
        var classes = element.GetAttribute("class");
        Assert.DoesNotContain("col-", classes);
        Assert.Contains("width: 25%", element.GetAttribute("style"));
    }

    [Fact]
    public void VisibleFalse_RendersNothing()
    {
        var cut = Render<TabPlaceholder>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void CustomCssClass_IsAppended()
    {
        var cut = Render<TabPlaceholder>(parameters => parameters
            .AddUnmatched("class", "my-custom-class"));

        Assert.Contains("my-custom-class", cut.Find("span").GetAttribute("class"));
    }

    [Fact]
    public void GeneratesUniqueId_WhenNoIdProvided()
    {
        var cut = Render<TabPlaceholder>();

        var id = cut.Find("span").GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("tab-", id);
    }

    [Fact]
    public void UsesProvidedId_WhenIdIsSet()
    {
        var cut = Render<TabPlaceholder>(parameters => parameters
            .AddUnmatched("id", "custom-placeholder-id"));

        Assert.Equal("custom-placeholder-id", cut.Find("span").GetAttribute("id"));
    }

    // -------------------------------------------------------------------------
    // TabPlaceholderContainer
    // -------------------------------------------------------------------------

    [Fact]
    public void Container_RendersDiv_ByDefault()
    {
        var cut = Render<TabPlaceholderContainer>();

        Assert.NotNull(cut.Find("div"));
    }

    [Fact]
    public void Container_AnimationGlow_AddsPlaceholderGlowClass()
    {
        var cut = Render<TabPlaceholderContainer>(parameters => parameters
            .Add(p => p.Animation, PlaceholderAnimation.Glow));

        Assert.Contains("placeholder-glow", cut.Find("div").GetAttribute("class"));
    }

    [Fact]
    public void Container_AnimationWave_AddsPlaceholderWaveClass()
    {
        var cut = Render<TabPlaceholderContainer>(parameters => parameters
            .Add(p => p.Animation, PlaceholderAnimation.Wave));

        Assert.Contains("placeholder-wave", cut.Find("div").GetAttribute("class"));
    }

    [Fact]
    public void Container_AnimationNone_DoesNotAddAnimationClass()
    {
        var cut = Render<TabPlaceholderContainer>(parameters => parameters
            .Add(p => p.Animation, PlaceholderAnimation.None));

        var classes = cut.Find("div").GetAttribute("class") ?? string.Empty;
        Assert.DoesNotContain("placeholder-glow", classes);
        Assert.DoesNotContain("placeholder-wave", classes);
    }

    [Fact]
    public void Container_AriaHiddenTrue_SetsAriaHiddenAttribute()
    {
        var cut = Render<TabPlaceholderContainer>(parameters => parameters
            .Add(p => p.AriaHidden, true));

        Assert.Equal("true", cut.Find("div").GetAttribute("aria-hidden"));
    }

    [Fact]
    public void Container_AriaHiddenFalse_OmitsAriaHiddenAttribute()
    {
        var cut = Render<TabPlaceholderContainer>(parameters => parameters
            .Add(p => p.AriaHidden, false));

        Assert.Null(cut.Find("div").GetAttribute("aria-hidden"));
    }

    [Fact]
    public void Container_TagP_RendersP()
    {
        var cut = Render<TabPlaceholderContainer>(parameters => parameters
            .Add(p => p.Tag, "p"));

        Assert.NotNull(cut.Find("p"));
    }

    [Fact]
    public void Container_TagSpan_RendersSpan()
    {
        var cut = Render<TabPlaceholderContainer>(parameters => parameters
            .Add(p => p.Tag, "span"));

        Assert.NotNull(cut.Find("span"));
    }

    [Fact]
    public void Container_RendersChildContent()
    {
        var cut = Render<TabPlaceholderContainer>(parameters => parameters
            .AddChildContent("<span class=\"placeholder col-6\"></span>"));

        Assert.NotNull(cut.Find("span"));
    }

    [Fact]
    public void Container_VisibleFalse_RendersNothing()
    {
        var cut = Render<TabPlaceholderContainer>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void Container_GeneratesUniqueId_WhenNoIdProvided()
    {
        var cut = Render<TabPlaceholderContainer>();

        var id = cut.Find("div").GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("tab-", id);
    }
}
