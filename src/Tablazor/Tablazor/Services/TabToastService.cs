using Tablazor.Enums;
using Tablazor.Models;

namespace Tablazor.Services;

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
