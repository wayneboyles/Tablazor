using Microsoft.AspNetCore.Components;
using Tablazor.Enums;
using Tablazor.Services;

namespace Tablazor.Components;

/// <summary>
/// A toast notification component based on the Tabler design system.
/// Displays a temporary message that can auto-dismiss and supports icons, colors, and custom content.
/// </summary>
/// <remarks>
/// Toasts are typically managed by <see cref="TabToastService"/> and displayed within a <see cref="TabToastContainer"/>.
/// For manual usage, provide the required parameters directly.
/// </remarks>
public partial class TabToast : TabBaseComponent
{
    private bool _isVisible;
    private bool _isShowing;
    private CancellationTokenSource? _autoCloseCts;
    private string _toastId = string.Empty;

    /// <summary>
    /// Gets or sets the toast instance when used with <see cref="TabToastService"/>.
    /// </summary>
    [Parameter]
    public ToastInstance? Toast { get; set; }

    /// <summary>
    /// Gets or sets the title text displayed in the toast header.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the message body of the toast.
    /// </summary>
    [Parameter]
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the content to render inside the toast body.
    /// Takes precedence over <see cref="Message"/> when provided.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the color theme of the toast.
    /// </summary>
    [Parameter]
    public TabColors Color { get; set; } = TabColors.Default;

    /// <summary>
    /// Gets or sets the icon markup to display in the toast header.
    /// Should be an SVG string or icon component markup.
    /// </summary>
    [Parameter]
    public string? Icon { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the close button is visible.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    [Parameter]
    public bool Closable { get; set; } = true;

    /// <summary>
    /// Gets or sets the duration in milliseconds before the toast auto-closes.
    /// Set to 0 or negative to disable auto-close.
    /// </summary>
    /// <value>The default value is 5000 milliseconds (5 seconds).</value>
    [Parameter]
    public int AutoCloseDelay { get; set; } = 5000;

    /// <summary>
    /// Gets or sets a value indicating whether the toast has a translucent background.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    [Parameter]
    public bool Translucent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether animations are enabled.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    [Parameter]
    public bool EnableAnimation { get; set; } = true;

    /// <summary>
    /// Gets or sets the callback invoked when the toast is closed.
    /// </summary>
    [Parameter]
    public EventCallback OnClose { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the toast close animation begins.
    /// Used internally by <see cref="TabToastContainer"/> to remove the toast.
    /// </summary>
    [Parameter]
    public EventCallback<ToastInstance> OnCloseRequested { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _toastId = GetId();
        _isVisible = true;

        // Apply options from ToastInstance if provided
        if (Toast is not null)
        {
            ApplyToastOptions(Toast.Options);
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            // Small delay to trigger CSS transition
            if (EnableAnimation)
            {
                await Task.Delay(10);
            }

            _isShowing = true;
            StateHasChanged();

            // Mark the toast instance as visible
            if (Toast is not null)
            {
                Toast.IsVisible = true;
            }

            // Start auto-close timer
            StartAutoCloseTimer();
        }
    }

    private void ApplyToastOptions(ToastOptions options)
    {
        Title = options.Title;
        Message = options.Message;
        Color = options.Color;
        Icon = options.Icon;
        Closable = options.Closable;
        AutoCloseDelay = options.AutoCloseDelay;
        Translucent = options.Translucent;
        ChildContent = options.Content;

        if (!string.IsNullOrEmpty(options.CssClass))
        {
            CssClass = options.CssClass;
        }
    }

    private void StartAutoCloseTimer()
    {
        if (AutoCloseDelay <= 0)
        {
            return;
        }

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
                // Timer was cancelled, ignore
            }
        });
    }

    /// <summary>
    /// Closes the toast with animation.
    /// </summary>
    public async Task CloseAsync()
    {
        if (!_isVisible)
        {
            return;
        }

        _autoCloseCts?.Cancel();

        // Start hide animation
        _isShowing = false;
        StateHasChanged();

        // Wait for animation
        if (EnableAnimation)
        {
            await Task.Delay(150);
        }

        _isVisible = false;
        StateHasChanged();

        // Notify parent container to remove this toast
        if (Toast is not null && OnCloseRequested.HasDelegate)
        {
            await OnCloseRequested.InvokeAsync(Toast);
        }

        if (OnClose.HasDelegate)
        {
            await OnClose.InvokeAsync();
        }
    }

    private async Task HandleCloseClickAsync()
    {
        await CloseAsync();
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("toast")
            .AddClass("fade", EnableAnimation)
            .AddClass("show", _isShowing)
            .AddClass("toast-translucent", Translucent)
            .AddClass(CssClass)
            .Build();
    }

    private string BuildHeaderCssClass()
    {
        return new CssBuilder("toast-header")
            .AddClass($"bg-{Color.GetCssClassName()}", Color != TabColors.Default)
            .AddClass("text-white", Color != TabColors.Default && Color != TabColors.Light && Color != TabColors.Warning)
            .Build();
    }

    private bool HasHeader => !string.IsNullOrEmpty(Title) || !string.IsNullOrEmpty(Icon) || Closable;

    private bool HasBody => !string.IsNullOrEmpty(Message) || ChildContent is not null;

    /// <inheritdoc />
    public override void Dispose()
    {
        _autoCloseCts?.Cancel();
        _autoCloseCts?.Dispose();
        base.Dispose();
    }
}
