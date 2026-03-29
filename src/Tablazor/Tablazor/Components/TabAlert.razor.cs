using Microsoft.AspNetCore.Components;

using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// An alert component that displays a contextual message to the user.
/// Supports colors, icons, titles, dismissal, auto-close, and action buttons.
/// </summary>
public partial class TabAlert : TabBaseComponent
{
    private bool _isVisible = true;
    private CancellationTokenSource? _autoCloseCts;

    /// <summary>
    /// Gets or sets the color theme of the alert.
    /// </summary>
    [Parameter]
    public TabColors Color { get; set; } = TabColors.Default;

    /// <summary>
    /// Gets or sets the alert title displayed above the message.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the alert message text.
    /// Ignored when <see cref="ChildContent"/> is provided.
    /// </summary>
    [Parameter]
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the custom body content of the alert.
    /// Takes precedence over <see cref="Message"/> when provided.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the SVG path data for the icon displayed alongside the alert content.
    /// Use icon path data from <see cref="Icons.TabIcons"/> (e.g., <c>TabIcons.Filled.CircleCheck</c>).
    /// </summary>
    [Parameter]
    public string? Icon { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a close button is shown.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    [Parameter]
    public bool Dismissible { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the alert automatically closes after <see cref="AutoCloseDelay"/> milliseconds.
    /// </summary>
    [Parameter]
    public bool AutoClose { get; set; }

    /// <summary>
    /// Gets or sets the duration in milliseconds before the alert auto-closes.
    /// Only applies when <see cref="AutoClose"/> is <c>true</c>.
    /// </summary>
    /// <value>The default value is 5000 milliseconds (5 seconds).</value>
    [Parameter]
    public int AutoCloseDelay { get; set; } = 5000;

    /// <summary>
    /// Gets or sets the action buttons rendered below the alert message.
    /// </summary>
    [Parameter]
    public RenderFragment? Actions { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the alert is closed.
    /// </summary>
    [Parameter]
    public EventCallback OnClose { get; set; }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender && AutoClose)
        {
            StartAutoCloseTimer();
        }
    }

    private void StartAutoCloseTimer()
    {
        if (AutoCloseDelay <= 0) return;

        _autoCloseCts?.Cancel();
        _autoCloseCts = new CancellationTokenSource();

        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(AutoCloseDelay, _autoCloseCts.Token);
                await InvokeAsync(CloseAsync);
            }
            catch (TaskCanceledException)
            {
                // Timer was cancelled
            }
        });
    }

    /// <summary>
    /// Closes the alert and fires <see cref="OnClose"/>.
    /// </summary>
    public async Task CloseAsync()
    {
        if (!_isVisible) return;

        _autoCloseCts?.Cancel();
        _isVisible = false;
        StateHasChanged();

        if (OnClose.HasDelegate)
        {
            await OnClose.InvokeAsync();
        }
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        _autoCloseCts?.Cancel();
        _autoCloseCts?.Dispose();
        base.Dispose();
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("alert")
            .AddClass($"alert-{Color.GetCssClassName()}", Color != TabColors.Default)
            .AddClass("alert-dismissible", Dismissible)
            .AddClass("d-flex align-items-center", !string.IsNullOrEmpty(Icon))
            .AddClass(CssClass)
            .Build();
    }
}
