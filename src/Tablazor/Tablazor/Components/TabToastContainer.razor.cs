using Microsoft.AspNetCore.Components;
using Tablazor.Enums;
using Tablazor.Services;

namespace Tablazor.Components;

/// <summary>
/// A container component that displays and manages toast notifications.
/// Place this component in your main layout to enable toast notifications throughout the application.
/// </summary>
/// <remarks>
/// The container subscribes to <see cref="TabToastService"/> and automatically displays
/// toasts when they are added. Toasts stack vertically and support positioning options.
/// </remarks>
/// <example>
/// <code>
/// &lt;!-- In MainLayout.razor --&gt;
/// &lt;TabToastContainer Position="ToastPosition.TopRight" /&gt;
/// </code>
/// </example>
public partial class TabToastContainer : TabBaseComponent, IDisposable
{
    private List<ToastInstance> _toasts = new();

    /// <summary>
    /// Gets or sets the toast service to subscribe to.
    /// </summary>
    [Inject]
    private TabToastService? ToastService { get; set; }

    /// <summary>
    /// Gets or sets the position where toasts are displayed.
    /// </summary>
    /// <value>The default value is <see cref="ToastPosition.TopRight"/>.</value>
    [Parameter]
    public ToastPosition Position { get; set; } = ToastPosition.TopRight;

    /// <summary>
    /// Gets or sets the maximum number of toasts to display at once.
    /// Older toasts will be removed when the limit is exceeded.
    /// Set to 0 or negative to disable the limit.
    /// </summary>
    /// <value>The default value is 5.</value>
    [Parameter]
    public int MaxToasts { get; set; } = 5;

    /// <summary>
    /// Gets or sets a value indicating whether animations are enabled for toasts.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    [Parameter]
    public bool EnableAnimation { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the newest toasts should appear at the top.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    [Parameter]
    public bool NewestOnTop { get; set; } = true;

    /// <summary>
    /// Gets or sets the z-index for the toast container.
    /// </summary>
    /// <value>The default value is 1090 (above Bootstrap modals).</value>
    [Parameter]
    public int ZIndex { get; set; } = 1090;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (ToastService is not null)
        {
            ToastService.OnChange += HandleToastsChanged;
            RefreshToasts();
        }
    }

    private void HandleToastsChanged()
    {
        RefreshToasts();
        InvokeAsync(StateHasChanged);
    }

    private void RefreshToasts()
    {
        if (ToastService is null)
        {
            return;
        }

        var toasts = ToastService.Toasts.ToList();

        // Enforce max toasts limit
        if (MaxToasts > 0 && toasts.Count > MaxToasts)
        {
            var toastsToRemove = toasts
                .OrderBy(t => t.CreatedAt)
                .Take(toasts.Count - MaxToasts)
                .ToList();

            foreach (var toast in toastsToRemove)
            {
                ToastService.Remove(toast);
            }

            toasts = ToastService.Toasts.ToList();
        }

        // Order by creation time
        _toasts = NewestOnTop
            ? toasts.OrderByDescending(t => t.CreatedAt).ToList()
            : toasts.OrderBy(t => t.CreatedAt).ToList();
    }

    private void HandleToastCloseRequested(ToastInstance toast)
    {
        ToastService?.Remove(toast);
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("toast-container")
            .AddClass("position-fixed")
            .AddClass("p-3")
            .AddClass(Position.GetCssClassName())
            .AddClass(CssClass)
            .Build();
    }

    /// <inheritdoc />
    protected override string BuildStyleString()
    {
        return new StyleBuilder()
            .AddStyle("z-index", ZIndex.ToString())
            .AddStyleFromAttributes(AdditionalAttributes)
            .Build();
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        if (ToastService is not null)
        {
            ToastService.OnChange -= HandleToastsChanged;
        }

        base.Dispose();
    }
}
