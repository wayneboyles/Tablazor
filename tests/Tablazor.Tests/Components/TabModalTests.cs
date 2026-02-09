using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Tablazor.Components;
using Tablazor.Enums;

namespace Tablazor.Tests.Components;

public class TabModalTests : BunitContext
{
    public TabModalTests()
    {
        Services.AddLogging();
        // Register empty JS interop module for tests
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    #region TabModal Tests

    [Fact]
    public void HasModalClass_WhenOpen()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true));

        var element = cut.Find("div.modal");
        Assert.Contains("modal", element.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotRender_WhenIsOpenIsFalse()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, false));

        var modals = cut.FindAll("div.modal");
        Assert.Empty(modals);
    }

    [Fact]
    public void HasFadeClass_WhenAnimationEnabled()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.EnableAnimation, true));

        var element = cut.Find("div.modal");
        Assert.Contains("fade", element.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotHaveFadeClass_WhenAnimationDisabled()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.EnableAnimation, false));

        var element = cut.Find("div.modal");
        Assert.DoesNotContain("fade", element.GetAttribute("class"));
    }

    [Fact]
    public void HasShowClass_WhenOpen()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true));

        var element = cut.Find("div.modal");
        Assert.Contains("show", element.GetAttribute("class"));
    }

    [Fact]
    public void RendersTitle_WhenTitleIsSet()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Title, "Test Modal Title"));

        var titleElement = cut.Find("h5.modal-title");
        Assert.Equal("Test Modal Title", titleElement.TextContent);
    }

    [Fact]
    public void RendersCloseButton_WhenShowCloseButtonIsTrue()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Title, "Test")
            .Add(p => p.ShowCloseButton, true));

        var closeButton = cut.Find("button.btn-close");
        Assert.NotNull(closeButton);
    }

    [Fact]
    public void DoesNotRenderCloseButton_WhenShowCloseButtonIsFalse()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Title, "Test")
            .Add(p => p.ShowCloseButton, false));

        var closeButtons = cut.FindAll("button.btn-close");
        Assert.Empty(closeButtons);
    }

    [Fact]
    public void AppliesSmallSizeClass_WhenSizeIsSmall()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Size, ModalSize.Small));

        var dialog = cut.Find("div.modal-dialog");
        Assert.Contains("modal-sm", dialog.GetAttribute("class"));
    }

    [Fact]
    public void AppliesLargeSizeClass_WhenSizeIsLarge()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Size, ModalSize.Large));

        var dialog = cut.Find("div.modal-dialog");
        Assert.Contains("modal-lg", dialog.GetAttribute("class"));
    }

    [Fact]
    public void AppliesFullWidthSizeClass_WhenSizeIsFullWidth()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Size, ModalSize.FullWidth));

        var dialog = cut.Find("div.modal-dialog");
        Assert.Contains("modal-full-width", dialog.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplySizeClass_WhenSizeIsDefault()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Size, ModalSize.Default));

        var dialog = cut.Find("div.modal-dialog");
        var classes = dialog.GetAttribute("class");
        Assert.DoesNotContain("modal-sm", classes);
        Assert.DoesNotContain("modal-lg", classes);
        Assert.DoesNotContain("modal-full-width", classes);
    }

    [Fact]
    public void AppliesCenteredClass_WhenCenteredIsTrue()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Centered, true));

        var dialog = cut.Find("div.modal-dialog");
        Assert.Contains("modal-dialog-centered", dialog.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyCenteredClass_WhenCenteredIsFalse()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Centered, false));

        var dialog = cut.Find("div.modal-dialog");
        Assert.DoesNotContain("modal-dialog-centered", dialog.GetAttribute("class"));
    }

    [Fact]
    public void AppliesScrollableClass_WhenScrollableIsTrue()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Scrollable, true));

        var dialog = cut.Find("div.modal-dialog");
        Assert.Contains("modal-dialog-scrollable", dialog.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplyScrollableClass_WhenScrollableIsFalse()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Scrollable, false));

        var dialog = cut.Find("div.modal-dialog");
        Assert.DoesNotContain("modal-dialog-scrollable", dialog.GetAttribute("class"));
    }

    [Fact]
    public void RendersChildContent()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .AddChildContent("<p>Modal content</p>"));

        cut.Find("div.modal-content").MarkupMatches("<div diff:ignoreAttributes><p>Modal content</p></div>");
    }

    [Fact]
    public void RendersFooterContent_WhenProvided()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.FooterContent, builder => builder.AddMarkupContent(0, "<button>OK</button>")));

        var footer = cut.Find("div.modal-footer");
        Assert.Contains("OK", footer.InnerHtml);
    }

    [Fact]
    public void DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void RendersBackdrop_WhenOpen()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true));

        var backdrop = cut.Find("div.modal-backdrop");
        Assert.NotNull(backdrop);
    }

    [Fact]
    public void HasRoleDialogAttribute()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true));

        var modal = cut.Find("div.modal");
        Assert.Equal("dialog", modal.GetAttribute("role"));
    }

    [Fact]
    public void HasTabIndexMinusOne()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true));

        var modal = cut.Find("div.modal");
        Assert.Equal("-1", modal.GetAttribute("tabindex"));
    }

    [Fact]
    public void HasAriaModalAttribute()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true));

        var modal = cut.Find("div.modal");
        Assert.Equal("true", modal.GetAttribute("aria-modal"));
    }

    [Fact]
    public void AppliesAdditionalAttributes()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .AddUnmatched("data-testid", "my-modal")
            .AddUnmatched("aria-describedby", "modal-description"));

        var modal = cut.Find("div.modal");
        Assert.Equal("my-modal", modal.GetAttribute("data-testid"));
        Assert.Equal("modal-description", modal.GetAttribute("aria-describedby"));
    }

    [Fact]
    public void GeneratesUniqueId_WhenNoIdProvided()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true));

        var modal = cut.Find("div.modal");
        var id = modal.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("tab-", id);
    }

    [Fact]
    public void UsesProvidedId_WhenIdIsSet()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .AddUnmatched("id", "custom-modal-id"));

        var modal = cut.Find("div.modal");
        Assert.Equal("custom-modal-id", modal.GetAttribute("id"));
    }

    [Fact]
    public void CombinesMultipleOptions()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Size, ModalSize.Large)
            .Add(p => p.Centered, true)
            .Add(p => p.Scrollable, true));

        var dialog = cut.Find("div.modal-dialog");
        var classes = dialog.GetAttribute("class");
        Assert.Contains("modal-lg", classes);
        Assert.Contains("modal-dialog-centered", classes);
        Assert.Contains("modal-dialog-scrollable", classes);
    }

    #endregion

    #region TabModalHeader Tests

    [Fact]
    public void TabModalHeader_HasModalHeaderClass()
    {
        var cut = Render<TabModalHeader>();

        var element = cut.Find("div");
        Assert.Contains("modal-header", element.GetAttribute("class"));
    }

    [Fact]
    public void TabModalHeader_RendersChildContent()
    {
        var cut = Render<TabModalHeader>(parameters => parameters
            .AddChildContent("<h5 class=\"modal-title\">Header</h5>"));

        Assert.NotNull(cut.Find("h5.modal-title"));
    }

    [Fact]
    public void TabModalHeader_RendersCloseButton_ByDefault()
    {
        var cut = Render<TabModalHeader>();

        var closeButton = cut.Find("button.btn-close");
        Assert.NotNull(closeButton);
    }

    [Fact]
    public void TabModalHeader_DoesNotRenderCloseButton_WhenShowCloseButtonIsFalse()
    {
        var cut = Render<TabModalHeader>(parameters => parameters
            .Add(p => p.ShowCloseButton, false));

        var closeButtons = cut.FindAll("button.btn-close");
        Assert.Empty(closeButtons);
    }

    [Fact]
    public void TabModalHeader_InvokesOnClose_WhenCloseButtonClicked()
    {
        var closeInvoked = false;
        var cut = Render<TabModalHeader>(parameters => parameters
            .Add(p => p.OnClose, EventCallback.Factory.Create(this, () => closeInvoked = true)));

        cut.Find("button.btn-close").Click();
        Assert.True(closeInvoked);
    }

    [Fact]
    public void TabModalHeader_DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabModalHeader>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    #endregion

    #region TabModalBody Tests

    [Fact]
    public void TabModalBody_HasModalBodyClass()
    {
        var cut = Render<TabModalBody>();

        var element = cut.Find("div");
        Assert.Contains("modal-body", element.GetAttribute("class"));
    }

    [Fact]
    public void TabModalBody_RendersChildContent()
    {
        var cut = Render<TabModalBody>(parameters => parameters
            .AddChildContent("<p>Body content</p>"));

        cut.Find("div").MarkupMatches("<div diff:ignoreAttributes><p>Body content</p></div>");
    }

    [Fact]
    public void TabModalBody_DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabModalBody>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void TabModalBody_AppliesCustomCssClass()
    {
        var cut = Render<TabModalBody>(parameters => parameters
            .AddUnmatched("class", "custom-body-class"));

        var element = cut.Find("div");
        Assert.Contains("custom-body-class", element.GetAttribute("class"));
    }

    #endregion

    #region TabModalFooter Tests

    [Fact]
    public void TabModalFooter_HasModalFooterClass()
    {
        var cut = Render<TabModalFooter>();

        var element = cut.Find("div");
        Assert.Contains("modal-footer", element.GetAttribute("class"));
    }

    [Fact]
    public void TabModalFooter_RendersChildContent()
    {
        var cut = Render<TabModalFooter>(parameters => parameters
            .AddChildContent("<button class=\"btn btn-primary\">Save</button>"));

        Assert.NotNull(cut.Find("button.btn.btn-primary"));
    }

    [Fact]
    public void TabModalFooter_DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabModalFooter>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void TabModalFooter_AppliesCustomCssClass()
    {
        var cut = Render<TabModalFooter>(parameters => parameters
            .AddUnmatched("class", "custom-footer-class"));

        var element = cut.Find("div");
        Assert.Contains("custom-footer-class", element.GetAttribute("class"));
    }

    #endregion

    #region TabModalTitle Tests

    [Fact]
    public void TabModalTitle_HasModalTitleClass()
    {
        var cut = Render<TabModalTitle>();

        var element = cut.Find("h5");
        Assert.Contains("modal-title", element.GetAttribute("class"));
    }

    [Fact]
    public void TabModalTitle_RendersAsH5_ByDefault()
    {
        var cut = Render<TabModalTitle>(parameters => parameters
            .AddChildContent("Title"));

        Assert.NotNull(cut.Find("h5"));
    }

    [Fact]
    public void TabModalTitle_RendersAsH1_WhenLevelIs1()
    {
        var cut = Render<TabModalTitle>(parameters => parameters
            .Add(p => p.Level, 1)
            .AddChildContent("Title"));

        Assert.NotNull(cut.Find("h1"));
    }

    [Fact]
    public void TabModalTitle_RendersAsH2_WhenLevelIs2()
    {
        var cut = Render<TabModalTitle>(parameters => parameters
            .Add(p => p.Level, 2)
            .AddChildContent("Title"));

        Assert.NotNull(cut.Find("h2"));
    }

    [Fact]
    public void TabModalTitle_RendersAsH3_WhenLevelIs3()
    {
        var cut = Render<TabModalTitle>(parameters => parameters
            .Add(p => p.Level, 3)
            .AddChildContent("Title"));

        Assert.NotNull(cut.Find("h3"));
    }

    [Fact]
    public void TabModalTitle_RendersAsH4_WhenLevelIs4()
    {
        var cut = Render<TabModalTitle>(parameters => parameters
            .Add(p => p.Level, 4)
            .AddChildContent("Title"));

        Assert.NotNull(cut.Find("h4"));
    }

    [Fact]
    public void TabModalTitle_RendersAsH6_WhenLevelIs6()
    {
        var cut = Render<TabModalTitle>(parameters => parameters
            .Add(p => p.Level, 6)
            .AddChildContent("Title"));

        Assert.NotNull(cut.Find("h6"));
    }

    [Fact]
    public void TabModalTitle_RendersChildContent()
    {
        var cut = Render<TabModalTitle>(parameters => parameters
            .AddChildContent("My Modal Title"));

        Assert.Equal("My Modal Title", cut.Find("h5").TextContent);
    }

    [Fact]
    public void TabModalTitle_DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabModalTitle>(parameters => parameters
            .Add(p => p.Visible, false));

        Assert.Empty(cut.Markup);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void TabModal_WithNestedComponents_RendersCorrectly()
    {
        var cut = Render<TabModal>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .AddChildContent(@"
                <div class=""modal-header"">
                    <h5 class=""modal-title"">Test Modal</h5>
                </div>
                <div class=""modal-body"">
                    <p>Modal body content</p>
                </div>
                <div class=""modal-footer"">
                    <button class=""btn btn-secondary"">Close</button>
                    <button class=""btn btn-primary"">Save</button>
                </div>
            "));

        Assert.NotNull(cut.Find(".modal-header"));
        Assert.NotNull(cut.Find(".modal-title"));
        Assert.NotNull(cut.Find(".modal-body"));
        Assert.NotNull(cut.Find(".modal-footer"));
    }

    #endregion
}
