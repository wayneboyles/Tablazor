using Microsoft.AspNetCore.Components;
using Tablazor.Enums;

namespace Tablazor.Services;

/// <summary>
/// Represents the configuration options for a toast notification.
/// </summary>
public sealed class ToastOptions
{
    /// <summary>
    /// Gets or sets the title/header text of the toast.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the message body of the toast.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the color theme of the toast.
    /// </summary>
    public TabColors Color { get; set; } = TabColors.Default;

    /// <summary>
    /// Gets or sets the icon to display in the toast header.
    /// This should be an SVG string or icon markup.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the toast can be manually closed by the user.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    public bool Closable { get; set; } = true;

    /// <summary>
    /// Gets or sets the duration in milliseconds before the toast automatically closes.
    /// Set to 0 or negative to disable auto-close.
    /// </summary>
    /// <value>The default value is 5000 milliseconds (5 seconds).</value>
    public int AutoCloseDelay { get; set; } = 5000;

    /// <summary>
    /// Gets or sets a value indicating whether the toast should have a translucent background.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    public bool Translucent { get; set; }

    /// <summary>
    /// Gets or sets custom content to render in the toast body.
    /// When set, this takes precedence over <see cref="Message"/>.
    /// </summary>
    public RenderFragment? Content { get; set; }

    /// <summary>
    /// Gets or sets a callback that is invoked when the toast is closed.
    /// </summary>
    public Action? OnClose { get; set; }

    /// <summary>
    /// Gets or sets additional CSS classes to apply to the toast.
    /// </summary>
    public string? CssClass { get; set; }
}

/// <summary>
/// Represents an active toast notification instance.
/// </summary>
public sealed class ToastInstance
{
    /// <summary>
    /// Gets the unique identifier for this toast instance.
    /// </summary>
    public string Id { get; } = Guid.NewGuid().ToString("N");

    /// <summary>
    /// Gets the timestamp when the toast was created.
    /// </summary>
    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the options for this toast.
    /// </summary>
    public ToastOptions Options { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the toast is currently visible (for animation purposes).
    /// </summary>
    internal bool IsVisible { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ToastInstance"/> class.
    /// </summary>
    /// <param name="options">The toast configuration options.</param>
    public ToastInstance(ToastOptions options)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
    }
}

/// <summary>
/// Service for displaying toast notifications throughout the application.
/// </summary>
/// <remarks>
/// Register this service using <c>services.AddTablazor()</c> and inject it where needed.
/// Place a <see cref="Components.TabToastContainer"/> component in your layout to display toasts.
/// </remarks>
/// <example>
/// <code>
/// @inject TabToastService ToastService
///
/// &lt;button @onclick="ShowToast"&gt;Show Toast&lt;/button&gt;
///
/// @code {
///     private void ShowToast()
///     {
///         ToastService.Show(new ToastOptions
///         {
///             Title = "Success",
///             Message = "Operation completed successfully!",
///             Color = TabColors.Success
///         });
///     }
/// }
/// </code>
/// </example>
public sealed class TabToastService
{
    private readonly List<ToastInstance> _toasts = new();
    private readonly object _lock = new();

    /// <summary>
    /// Raised when the toast collection changes (toast added or removed).
    /// </summary>
    public event Action? OnChange;

    /// <summary>
    /// Gets a read-only collection of currently active toasts.
    /// </summary>
    public IReadOnlyList<ToastInstance> Toasts
    {
        get
        {
            lock (_lock)
            {
                return _toasts.ToList().AsReadOnly();
            }
        }
    }

    /// <summary>
    /// Shows a toast notification with the specified options.
    /// </summary>
    /// <param name="options">The toast configuration options.</param>
    /// <returns>The created toast instance.</returns>
    public ToastInstance Show(ToastOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var toast = new ToastInstance(options);

        lock (_lock)
        {
            _toasts.Add(toast);
        }

        NotifyStateChanged();
        return toast;
    }

    /// <summary>
    /// Shows a simple toast with just a message.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="color">The optional color theme.</param>
    /// <returns>The created toast instance.</returns>
    public ToastInstance Show(string message, TabColors color = TabColors.Default)
    {
        return Show(new ToastOptions
        {
            Message = message,
            Color = color
        });
    }

    /// <summary>
    /// Shows a toast with a title and message.
    /// </summary>
    /// <param name="title">The toast header title.</param>
    /// <param name="message">The message to display.</param>
    /// <param name="color">The optional color theme.</param>
    /// <returns>The created toast instance.</returns>
    public ToastInstance Show(string title, string message, TabColors color = TabColors.Default)
    {
        return Show(new ToastOptions
        {
            Title = title,
            Message = message,
            Color = color
        });
    }

    /// <summary>
    /// Shows a success toast notification.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The optional title.</param>
    /// <returns>The created toast instance.</returns>
    public ToastInstance ShowSuccess(string message, string? title = null)
    {
        return Show(new ToastOptions
        {
            Title = title ?? "Success",
            Message = message,
            Color = TabColors.Success
        });
    }

    /// <summary>
    /// Shows an error toast notification.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The optional title.</param>
    /// <returns>The created toast instance.</returns>
    public ToastInstance ShowError(string message, string? title = null)
    {
        return Show(new ToastOptions
        {
            Title = title ?? "Error",
            Message = message,
            Color = TabColors.Danger
        });
    }

    /// <summary>
    /// Shows a warning toast notification.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The optional title.</param>
    /// <returns>The created toast instance.</returns>
    public ToastInstance ShowWarning(string message, string? title = null)
    {
        return Show(new ToastOptions
        {
            Title = title ?? "Warning",
            Message = message,
            Color = TabColors.Warning
        });
    }

    /// <summary>
    /// Shows an info toast notification.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The optional title.</param>
    /// <returns>The created toast instance.</returns>
    public ToastInstance ShowInfo(string message, string? title = null)
    {
        return Show(new ToastOptions
        {
            Title = title ?? "Info",
            Message = message,
            Color = TabColors.Info
        });
    }

    /// <summary>
    /// Removes a specific toast from the display.
    /// </summary>
    /// <param name="toast">The toast instance to remove.</param>
    public void Remove(ToastInstance toast)
    {
        ArgumentNullException.ThrowIfNull(toast);

        bool removed;
        lock (_lock)
        {
            removed = _toasts.Remove(toast);
        }

        if (removed)
        {
            toast.Options.OnClose?.Invoke();
            NotifyStateChanged();
        }
    }

    /// <summary>
    /// Removes a toast by its unique identifier.
    /// </summary>
    /// <param name="toastId">The ID of the toast to remove.</param>
    public void Remove(string toastId)
    {
        ToastInstance? toast;
        lock (_lock)
        {
            toast = _toasts.Find(t => t.Id == toastId);
        }

        if (toast is not null)
        {
            Remove(toast);
        }
    }

    /// <summary>
    /// Removes all currently displayed toasts.
    /// </summary>
    public void Clear()
    {
        List<ToastInstance> toastsToRemove;
        lock (_lock)
        {
            toastsToRemove = _toasts.ToList();
            _toasts.Clear();
        }

        foreach (var toast in toastsToRemove)
        {
            toast.Options.OnClose?.Invoke();
        }

        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
