using System.Runtime.CompilerServices;

namespace Tablazor;

internal static class IdBuilder
{
    private static long _counter = 0;
    private static readonly string Prefix = "tab";

    private const string Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// Gets the current counter value without incrementing.
    /// </summary>
    public static long CurrentCount => Interlocked.Read(ref _counter);

    /// <summary>
    /// Generates a unique HTML component ID using an atomic counter.
    /// Thread-safe and lock-free for high concurrency scenarios.
    /// </summary>
    /// <returns>A unique ID string (e.g., "cmp-1", "cmp-2")</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Next()
    {
        var id = Interlocked.Increment(ref _counter);
        return $"{Prefix}-{id}";
    }

    /// <summary>
    /// Generates a unique HTML component ID with a custom prefix.
    /// </summary>
    /// <param name="prefix">Custom prefix for the ID</param>
    /// <returns>A unique ID string with custom prefix</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Next(string prefix)
    {
        var id = Interlocked.Increment(ref _counter);
        return $"{prefix}-{id}";
    }

    /// <summary>
    /// Generates a short unique ID using Base62 encoding for compact representations.
    /// Produces shorter IDs like "cmp-a", "cmp-1B", "cmp-3z" etc.
    /// </summary>
    /// <returns>A unique compact ID string</returns>
    public static string NextCompact()
    {
        var id = Interlocked.Increment(ref _counter);
        return $"{Prefix}-{ToBase62(id)}";
    }

    /// <summary>
    /// Generates a unique HTML component ID with type-based prefix.
    /// </summary>
    /// <param name="componentType">Type of the component</param>
    /// <returns>A unique ID string with type-based prefix</returns>
    public static string NextForType(Type componentType)
    {
        var id = Interlocked.Increment(ref _counter);
        var typeName = componentType.Name.ToLowerInvariant();
        return $"{typeName}-{id}";
    }

    /// <summary>
    /// Resets the internal counter. Use with caution in production environments.
    /// Primarily intended for testing scenarios.
    /// </summary>
    public static void Reset()
    {
        Interlocked.Exchange(ref _counter, 0);
    }

    private static string ToBase62(long value)
    {
        if (value == 0)
        {
            return "0";
        }

        Span<char> buffer = stackalloc char[11]; // Max length for long in base62
        var idx = buffer.Length;

        while (value > 0)
        {
            buffer[--idx] = Base62Chars[(int)(value % 62)];
            value /= 62;
        }

        return new string(buffer.Slice(idx));
    }
}