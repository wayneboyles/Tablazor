namespace Tablazor.Models;

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