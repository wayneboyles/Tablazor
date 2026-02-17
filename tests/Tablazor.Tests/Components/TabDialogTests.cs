using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Tablazor.Components;
using Tablazor.Enums;

namespace Tablazor.Tests.Components;

public class TabDialogTests : BunitContext
{
    public TabDialogTests()
    {
        Services.AddLogging();
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    /// <summary>
    /// Helper to show the dialog on the bUnit dispatcher without blocking.
    /// Returns the Task&lt;DialogResult&gt; for tests that need to await the result.
    /// </summary>
    private Task<DialogResult> ShowDialog(IRenderedComponent<TabDialog> cut)
    {
        Task<DialogResult>? resultTask = null;
        cut.InvokeAsync(() =>
        {
            resultTask = cut.Instance.ShowAsync();
        });
        return resultTask!;
    }

    #region Rendering Tests

    [Fact]
    public void DoesNotRender_WhenNotShown()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test"));

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void RendersModal_WhenShowAsyncCalled()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test Dialog")
            .Add(p => p.Message, "Test message"));

        ShowDialog(cut);

        var modal = cut.Find("div.modal");
        Assert.NotNull(modal);
    }

    [Fact]
    public void RendersTitle_InModalHeader()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "My Dialog Title")
            .Add(p => p.Message, "Message"));

        ShowDialog(cut);

        var title = cut.Find("h5.modal-title");
        Assert.Equal("My Dialog Title", title.TextContent);
    }

    [Fact]
    public void RendersMessage_InModalBody()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Message, "This is a test message"));

        ShowDialog(cut);

        var body = cut.Find("div.modal-body");
        Assert.Contains("This is a test message", body.TextContent);
    }

    [Fact]
    public void RendersMessageContent_WhenProvided()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.MessageContent, builder =>
                builder.AddMarkupContent(0, "<strong>Rich content</strong>")));

        ShowDialog(cut);

        var body = cut.Find("div.modal-body");
        Assert.Contains("Rich content", body.InnerHtml);
        Assert.Contains("<strong>", body.InnerHtml);
    }

    [Fact]
    public void MessageContent_TakesPrecedence_OverMessage()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Message, "Plain text")
            .Add(p => p.MessageContent, builder =>
                builder.AddMarkupContent(0, "<em>Rich text</em>")));

        ShowDialog(cut);

        var body = cut.Find("div.modal-body");
        Assert.Contains("Rich text", body.InnerHtml);
        Assert.DoesNotContain("Plain text", body.InnerHtml);
    }

    [Fact]
    public void DoesNotRender_WhenVisibleIsFalse()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Visible, false));

        ShowDialog(cut);

        Assert.Empty(cut.Markup);
    }

    #endregion

    #region Button Configuration Tests

    [Fact]
    public void RendersOkButton_WhenButtonsIsOk()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Buttons, DialogButtons.Ok));

        ShowDialog(cut);

        var buttons = cut.FindAll("div.modal-footer button");
        Assert.Single(buttons);
        Assert.Equal("OK", buttons[0].TextContent.Trim());
    }

    [Fact]
    public void RendersCancelButton_WhenButtonsIsCancel()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Buttons, DialogButtons.Cancel));

        ShowDialog(cut);

        var buttons = cut.FindAll("div.modal-footer button");
        Assert.Single(buttons);
        Assert.Equal("Cancel", buttons[0].TextContent.Trim());
    }

    [Fact]
    public void RendersOkAndCancelButtons_WhenButtonsIsOkCancel()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Buttons, DialogButtons.OkCancel));

        ShowDialog(cut);

        var buttons = cut.FindAll("div.modal-footer button");
        Assert.Equal(2, buttons.Count);
        Assert.Equal("OK", buttons[0].TextContent.Trim());
        Assert.Equal("Cancel", buttons[1].TextContent.Trim());
    }

    [Fact]
    public void RendersYesAndNoButtons_WhenButtonsIsYesNo()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Buttons, DialogButtons.YesNo));

        ShowDialog(cut);

        var buttons = cut.FindAll("div.modal-footer button");
        Assert.Equal(2, buttons.Count);
        Assert.Equal("Yes", buttons[0].TextContent.Trim());
        Assert.Equal("No", buttons[1].TextContent.Trim());
    }

    [Fact]
    public void RendersYesNoCancelButtons_WhenButtonsIsYesNoCancel()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Buttons, DialogButtons.YesNoCancel));

        ShowDialog(cut);

        var buttons = cut.FindAll("div.modal-footer button");
        Assert.Equal(3, buttons.Count);
        Assert.Equal("Yes", buttons[0].TextContent.Trim());
        Assert.Equal("No", buttons[1].TextContent.Trim());
        Assert.Equal("Cancel", buttons[2].TextContent.Trim());
    }

    [Fact]
    public void UsesCustomButtonText()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Buttons, DialogButtons.YesNoCancel)
            .Add(p => p.YesText, "Confirm")
            .Add(p => p.NoText, "Deny")
            .Add(p => p.CancelText, "Abort"));

        ShowDialog(cut);

        var buttons = cut.FindAll("div.modal-footer button");
        Assert.Equal("Confirm", buttons[0].TextContent.Trim());
        Assert.Equal("Deny", buttons[1].TextContent.Trim());
        Assert.Equal("Abort", buttons[2].TextContent.Trim());
    }

    [Fact]
    public void OkButton_HasPrimaryClass()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Buttons, DialogButtons.Ok));

        ShowDialog(cut);

        var button = cut.Find("div.modal-footer button");
        Assert.Contains("btn-primary", button.GetAttribute("class"));
    }

    [Fact]
    public void CancelButton_HasSecondaryClass()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Buttons, DialogButtons.Cancel));

        ShowDialog(cut);

        var button = cut.Find("div.modal-footer button");
        Assert.Contains("btn-secondary", button.GetAttribute("class"));
    }

    [Fact]
    public void YesButton_HasPrimaryClass()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Buttons, DialogButtons.YesNo));

        ShowDialog(cut);

        var buttons = cut.FindAll("div.modal-footer button");
        Assert.Contains("btn-primary", buttons[0].GetAttribute("class"));
    }

    [Fact]
    public void NoButton_HasSecondaryClass()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Buttons, DialogButtons.YesNo));

        ShowDialog(cut);

        var buttons = cut.FindAll("div.modal-footer button");
        Assert.Contains("btn-secondary", buttons[1].GetAttribute("class"));
    }

    #endregion

    #region DialogResult Tests

    [Fact]
    public async Task ShowAsync_ReturnsOk_WhenOkClicked()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Buttons, DialogButtons.Ok));

        var resultTask = ShowDialog(cut);

        cut.Find("div.modal-footer button").Click();

        var result = await resultTask;
        Assert.Equal(DialogResult.Ok, result);
    }

    [Fact]
    public async Task ShowAsync_ReturnsCancel_WhenCancelClicked()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Buttons, DialogButtons.OkCancel));

        var resultTask = ShowDialog(cut);

        var buttons = cut.FindAll("div.modal-footer button");
        buttons[1].Click();

        var result = await resultTask;
        Assert.Equal(DialogResult.Cancel, result);
    }

    [Fact]
    public async Task ShowAsync_ReturnsYes_WhenYesClicked()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Buttons, DialogButtons.YesNo));

        var resultTask = ShowDialog(cut);

        var buttons = cut.FindAll("div.modal-footer button");
        buttons[0].Click();

        var result = await resultTask;
        Assert.Equal(DialogResult.Yes, result);
    }

    [Fact]
    public async Task ShowAsync_ReturnsNo_WhenNoClicked()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Buttons, DialogButtons.YesNo));

        var resultTask = ShowDialog(cut);

        var buttons = cut.FindAll("div.modal-footer button");
        buttons[1].Click();

        var result = await resultTask;
        Assert.Equal(DialogResult.No, result);
    }

    [Fact]
    public async Task ShowAsync_ReturnsNone_WhenEscapePressed()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Buttons, DialogButtons.Ok));

        var resultTask = ShowDialog(cut);

        cut.Find("div.modal").KeyDown(Key.Escape);

        var result = await resultTask;
        Assert.Equal(DialogResult.None, result);
    }

    [Fact]
    public void Close_HidesDialog()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Buttons, DialogButtons.Ok));

        ShowDialog(cut);
        Assert.NotEmpty(cut.FindAll("div.modal"));

        cut.InvokeAsync(() => cut.Instance.Close(DialogResult.Ok));

        Assert.Empty(cut.FindAll("div.modal"));
    }

    [Fact]
    public void OnResult_IsInvoked_WhenButtonClicked()
    {
        DialogResult? receivedResult = null;
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Buttons, DialogButtons.Ok)
            .Add(p => p.OnResult, EventCallback.Factory.Create<DialogResult>(this, r => receivedResult = r)));

        ShowDialog(cut);

        cut.Find("div.modal-footer button").Click();

        Assert.Equal(DialogResult.Ok, receivedResult);
    }

    [Fact]
    public async Task ShowAsync_CancelsPreviousDialog_WhenCalledAgain()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Buttons, DialogButtons.Ok));

        var firstTask = ShowDialog(cut);
        var secondTask = ShowDialog(cut);

        // First task should resolve with None (cancelled)
        var firstResult = await firstTask;
        Assert.Equal(DialogResult.None, firstResult);

        // Second task should still be pending until a button is clicked
        Assert.False(secondTask.IsCompleted);

        cut.Find("div.modal-footer button").Click();

        var secondResult = await secondTask;
        Assert.Equal(DialogResult.Ok, secondResult);
    }

    #endregion

    #region Size Tests

    [Fact]
    public void AppliesSmallSizeClass_WhenSizeIsSmall()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Size, ModalSize.Small));

        ShowDialog(cut);

        var dialog = cut.Find("div.modal-dialog");
        Assert.Contains("modal-sm", dialog.GetAttribute("class"));
    }

    [Fact]
    public void AppliesLargeSizeClass_WhenSizeIsLarge()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Size, ModalSize.Large));

        ShowDialog(cut);

        var dialog = cut.Find("div.modal-dialog");
        Assert.Contains("modal-lg", dialog.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotApplySizeClass_WhenSizeIsDefault()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Size, ModalSize.Default));

        ShowDialog(cut);

        var dialog = cut.Find("div.modal-dialog");
        var classes = dialog.GetAttribute("class");
        Assert.DoesNotContain("modal-sm", classes);
        Assert.DoesNotContain("modal-lg", classes);
    }

    #endregion

    #region Color / Status Bar Tests

    [Fact]
    public void RendersStatusBar_WhenColorIsSet()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Color, TabColors.Danger));

        ShowDialog(cut);

        var statusBar = cut.Find("div.modal-status");
        Assert.Contains("bg-danger", statusBar.GetAttribute("class"));
    }

    [Fact]
    public void DoesNotRenderStatusBar_WhenColorIsDefault()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Color, TabColors.Default));

        ShowDialog(cut);

        var statusBars = cut.FindAll("div.modal-status");
        Assert.Empty(statusBars);
    }

    [Fact]
    public void RendersStatusBar_WithCorrectColor()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Color, TabColors.Success));

        ShowDialog(cut);

        var statusBar = cut.Find("div.modal-status");
        Assert.Contains("bg-success", statusBar.GetAttribute("class"));
    }

    #endregion

    #region Accessibility Tests

    [Fact]
    public void HasAlertDialogRole()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test"));

        ShowDialog(cut);

        var modal = cut.Find("div.modal");
        Assert.Equal("alertdialog", modal.GetAttribute("role"));
    }

    [Fact]
    public void HasAriaModalAttribute()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test"));

        ShowDialog(cut);

        var modal = cut.Find("div.modal");
        Assert.Equal("true", modal.GetAttribute("aria-modal"));
    }

    [Fact]
    public void HasAriaLabelledBy_PointingToTitle()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test"));

        ShowDialog(cut);

        var modal = cut.Find("div.modal");
        var titleId = cut.Find("h5.modal-title").GetAttribute("id");
        Assert.Equal(titleId, modal.GetAttribute("aria-labelledby"));
    }

    [Fact]
    public void HasAriaDescribedBy_PointingToMessage()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .Add(p => p.Message, "A message"));

        ShowDialog(cut);

        var modal = cut.Find("div.modal");
        var bodyId = cut.Find("div.modal-body").GetAttribute("id");
        Assert.Equal(bodyId, modal.GetAttribute("aria-describedby"));
    }

    [Fact]
    public void HasTabIndexMinusOne()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test"));

        ShowDialog(cut);

        var modal = cut.Find("div.modal");
        Assert.Equal("-1", modal.GetAttribute("tabindex"));
    }

    #endregion

    #region CSS Class Tests

    [Fact]
    public void HasModalClass_WhenOpen()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test"));

        ShowDialog(cut);

        var modal = cut.Find("div.modal");
        Assert.Contains("modal", modal.GetAttribute("class"));
    }

    [Fact]
    public void HasShowClass_WhenOpen()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test"));

        ShowDialog(cut);

        var modal = cut.Find("div.modal");
        Assert.Contains("show", modal.GetAttribute("class"));
    }

    [Fact]
    public void HasModalBlurClass_WhenOpen()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test"));

        ShowDialog(cut);

        var modal = cut.Find("div.modal");
        Assert.Contains("modal-blur", modal.GetAttribute("class"));
    }

    [Fact]
    public void HasCenteredDialogClass()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test"));

        ShowDialog(cut);

        var dialog = cut.Find("div.modal-dialog");
        Assert.Contains("modal-dialog-centered", dialog.GetAttribute("class"));
    }

    [Fact]
    public void RendersBackdrop_WhenOpen()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test"));

        ShowDialog(cut);

        var backdrop = cut.Find("div.modal-backdrop");
        Assert.NotNull(backdrop);
    }

    [Fact]
    public void AppliesAdditionalAttributes()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test")
            .AddUnmatched("data-testid", "my-dialog"));

        ShowDialog(cut);

        var modal = cut.Find("div.modal");
        Assert.Equal("my-dialog", modal.GetAttribute("data-testid"));
    }

    [Fact]
    public void GeneratesUniqueId_WhenNoIdProvided()
    {
        var cut = Render<TabDialog>(parameters => parameters
            .Add(p => p.Title, "Test"));

        ShowDialog(cut);

        var modal = cut.Find("div.modal");
        var id = modal.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("tab-", id);
    }

    #endregion
}
