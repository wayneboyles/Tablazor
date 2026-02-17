using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// A dialog component for displaying messages with standard dialog buttons (OK, Cancel, Yes, No).
/// Uses Tabler modal CSS classes and provides an awaitable <see cref="ShowAsync"/> method
/// that resolves with a <see cref="DialogResult"/> when the user clicks a button.
/// </summary>
/// <remarks>
/// Unlike <see cref="TabModal"/>, this component is designed for simple message dialogs
/// with predefined button configurations. Use <see cref="DialogButtons"/> to specify
/// which buttons to display. The dialog returns a <see cref="DialogResult"/> indicating
/// which button was clicked.
/// </remarks>
public partial class TabDialog : TabBaseComponent
{
    private string _dialogId = string.Empty;
    private bool _isOpen;
    private TaskCompletionSource<DialogResult>? _tcs;

    /// <summary>
    /// Gets or sets the title to display in the dialog header.
    /// </summary>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the message text to display in the dialog body.
    /// For rich content, use <see cref="MessageContent"/> instead.
    /// </summary>
    [Parameter]
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the render fragment to display in the dialog body.
    /// Takes precedence over <see cref="Message"/> when both are provided.
    /// </summary>
    [Parameter]
    public RenderFragment? MessageContent { get; set; }

    /// <summary>
    /// Gets or sets which buttons to display in the dialog footer.
    /// </summary>
    /// <value>The default value is <see cref="DialogButtons.Ok"/>.</value>
    [Parameter]
    public DialogButtons Buttons { get; set; } = DialogButtons.Ok;

    /// <summary>
    /// Gets or sets the size of the dialog.
    /// </summary>
    /// <value>The default value is <see cref="ModalSize.Small"/>.</value>
    [Parameter]
    public ModalSize Size { get; set; } = ModalSize.Small;

    /// <summary>
    /// Gets or sets the color for the status bar at the top of the dialog.
    /// When set to a value other than <see cref="TabColors.Default"/>, a colored
    /// status bar will be displayed at the top of the dialog.
    /// </summary>
    /// <value>The default value is <see cref="TabColors.Default"/>.</value>
    [Parameter]
    public TabColors Color { get; set; } = TabColors.Default;

    /// <summary>
    /// Gets or sets the text for the OK button.
    /// </summary>
    /// <value>The default value is "OK".</value>
    [Parameter]
    public string OkText { get; set; } = "OK";

    /// <summary>
    /// Gets or sets the text for the Cancel button.
    /// </summary>
    /// <value>The default value is "Cancel".</value>
    [Parameter]
    public string CancelText { get; set; } = "Cancel";

    /// <summary>
    /// Gets or sets the text for the Yes button.
    /// </summary>
    /// <value>The default value is "Yes".</value>
    [Parameter]
    public string YesText { get; set; } = "Yes";

    /// <summary>
    /// Gets or sets the text for the No button.
    /// </summary>
    /// <value>The default value is "No".</value>
    [Parameter]
    public string NoText { get; set; } = "No";

    /// <summary>
    /// Gets or sets the callback that is invoked when the dialog is closed with a result.
    /// </summary>
    [Parameter]
    public EventCallback<DialogResult> OnResult { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _dialogId = GetId();
    }

    /// <summary>
    /// Shows the dialog and returns the result when a button is clicked.
    /// </summary>
    /// <returns>A <see cref="Task{DialogResult}"/> that resolves when the user clicks a button.</returns>
    public async Task<DialogResult> ShowAsync()
    {
        // Cancel any previous pending dialog
        _tcs?.TrySetResult(DialogResult.None);

        _tcs = new TaskCompletionSource<DialogResult>();
        _isOpen = true;
        await InvokeAsync(StateHasChanged);

        return await _tcs.Task;
    }

    /// <summary>
    /// Closes the dialog with the specified result.
    /// </summary>
    /// <param name="result">The dialog result indicating which button was clicked.</param>
    public void Close(DialogResult result)
    {
        _isOpen = false;
        _tcs?.TrySetResult(result);
        _tcs = null;
        StateHasChanged();

        if (OnResult.HasDelegate)
        {
            _ = OnResult.InvokeAsync(result);
        }
    }

    /// <summary>
    /// Handles keyboard events for the dialog.
    /// Pressing Escape closes the dialog with <see cref="DialogResult.None"/>.
    /// </summary>
    private void HandleKeyDownAsync(KeyboardEventArgs e)
    {
        if (e.Key == "Escape")
        {
            Close(DialogResult.None);
        }
    }

    /// <summary>
    /// Builds the CSS class string for the dialog wrapper element.
    /// </summary>
    /// <returns>A string containing the combined CSS classes.</returns>
    protected override string BuildCssClass()
    {
        return new CssBuilder("modal modal-blur show")
            .AddClass(CssClass)
            .Build();
    }

    /// <summary>
    /// Builds the CSS class string for the modal dialog element.
    /// </summary>
    /// <returns>A string containing the combined CSS classes for the dialog.</returns>
    private string BuildDialogCssClass()
    {
        return new CssBuilder("modal-dialog modal-dialog-centered")
            .AddClass($"modal-{Size.GetCssClassName()}", Size != ModalSize.Default)
            .Build();
    }
}
