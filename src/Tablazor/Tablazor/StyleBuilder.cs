using System.Text;

namespace Tablazor;

/// <summary>
/// Represents a builder for creating inline CSS styles used in a component.
/// </summary>
public struct StyleBuilder
{
    private StringBuilder? _stringBuilder;

    /// <summary>
    /// Creates a new instance of StyleBuilder with the specified initial value.
    /// </summary>
    /// <remarks>
    /// Call <see cref="Build"/> to return the completed CSS styles as a string.
    /// </remarks>
    /// <param name="value">The initial CSS style value.</param>
    /// <returns>The <see cref="StyleBuilder"/> instance.</returns>
    public static StyleBuilder Default(string value) => new(value);

    /// <summary>
    /// Creates an empty instance of StyleBuilder.
    /// </summary>
    /// <remarks>
    /// Call <see cref="Build"/> to return the completed CSS styles as a string.
    /// </remarks>
    /// <returns>The <see cref="StyleBuilder"/> instance.</returns>
    public static StyleBuilder Empty() => new();

    /// <summary>
    /// Creates an empty instance of StyleBuilder.
    /// </summary>
    /// <remarks>
    /// Call <see cref="Build"/> to return the completed CSS styles as a string.
    /// </remarks>
    /// <returns>The <see cref="StyleBuilder"/> instance.</returns>
    public StyleBuilder()
    {
        _stringBuilder = EnsureCreated();
    }

    /// <summary>
    /// Initializes a new instance of the StyleBuilder class with the specified initial value.
    /// </summary>
    /// <remarks>
    /// Call <see cref="Build"/> to return the completed CSS styles as a string.
    /// </remarks>
    /// <param name="value">The initial CSS style value.</param>
    /// <returns>The <see cref="StyleBuilder"/> instance.</returns>
    public StyleBuilder(string? value) : this()
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            EnsureCreated().Append(value);

            if (!value.EndsWith(';'))
            {
                EnsureCreated().Append(';');
            }
        }
    }

    /// <summary>
    /// Adds a raw string to the builder that will be concatenated with the next style added to the builder.
    /// </summary>
    /// <param name="value">The string value to add.</param>
    /// <returns>The <see cref="StyleBuilder"/> instance.</returns>
    public StyleBuilder AddValue(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            EnsureCreated().Append(value);
        }
        return this;
    }

    /// <summary>
    /// Adds a CSS style to the builder.
    /// </summary>
    /// <param name="property">The CSS property name.</param>
    /// <param name="value">The CSS property value.</param>
    /// <returns>The <see cref="StyleBuilder"/> instance.</returns>
    public StyleBuilder AddStyle(string property, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            EnsureCreated().Append(property).Append(": ").Append(value).Append("; ");
        }
        return this;
    }

    /// <summary>
    /// Adds a conditional CSS style to the builder.
    /// </summary>
    /// <param name="property">The CSS property name.</param>
    /// <param name="value">The CSS property value.</param>
    /// <param name="when">The condition in which the CSS style is added.</param>
    /// <returns>The <see cref="StyleBuilder"/> instance.</returns>
    public StyleBuilder AddStyle(string property, string? value, bool when)
        => when ? AddStyle(property, value) : this;

    /// <summary>
    /// Adds a conditional CSS style to the builder.
    /// </summary>
    /// <param name="property">The CSS property name.</param>
    /// <param name="value">The CSS property value.</param>
    /// <param name="when">The nullable condition in which the CSS style is added.</param>
    /// <returns>The <see cref="StyleBuilder"/> instance.</returns>
    public StyleBuilder AddStyle(string property, string? value, bool? when)
        => when == true ? AddStyle(property, value) : this;

    /// <summary>
    /// Adds a conditional CSS style to the builder.
    /// </summary>
    /// <param name="property">The CSS property name.</param>
    /// <param name="value">The CSS property value.</param>
    /// <param name="when">The condition function in which the CSS style is added.</param>
    /// <returns>The <see cref="StyleBuilder"/> instance.</returns>
    public StyleBuilder AddStyle(string property, string? value, Func<bool>? when)
        => AddStyle(property, value, when is not null && when());

    /// <summary>
    /// Adds a conditional CSS style to the builder.
    /// </summary>
    /// <param name="property">The CSS property name.</param>
    /// <param name="value">A function that returns the CSS property value.</param>
    /// <param name="when">The condition in which the CSS style is added.</param>
    /// <returns>The <see cref="StyleBuilder"/> instance.</returns>
    public StyleBuilder AddStyle(string property, Func<string?> value, bool when = true)
        => when ? AddStyle(property, value()) : this;

    /// <summary>
    /// Adds a conditional CSS style to the builder.
    /// </summary>
    /// <param name="property">The CSS property name.</param>
    /// <param name="value">A function that returns the CSS property value.</param>
    /// <param name="when">The condition function in which the CSS style is added.</param>
    /// <returns>The <see cref="StyleBuilder"/> instance.</returns>
    public StyleBuilder AddStyle(string property, Func<string?> value, Func<bool>? when)
        => AddStyle(property, value, when is not null && when());

    /// <summary>
    /// Adds a raw CSS style string to the builder.
    /// </summary>
    /// <param name="style">The CSS style string (e.g., "color: red" or "color: red;").</param>
    /// <returns>The <see cref="StyleBuilder"/> instance.</returns>
    public StyleBuilder AddRaw(string? style)
    {
        if (!string.IsNullOrWhiteSpace(style))
        {
            EnsureCreated().Append(style);

            if (!style.EndsWith(';'))
            {
                EnsureCreated().Append(';');
            }

            EnsureCreated().Append(' ');
        }
        return this;
    }

    /// <summary>
    /// Adds a conditional raw CSS style string to the builder.
    /// </summary>
    /// <param name="style">The CSS style string.</param>
    /// <param name="when">The condition in which the CSS style is added.</param>
    /// <returns>The <see cref="StyleBuilder"/> instance.</returns>
    public StyleBuilder AddRaw(string? style, bool when) => when ? AddRaw(style) : this;

    /// <summary>
    /// Adds a conditional raw CSS style string to the builder.
    /// </summary>
    /// <param name="style">The CSS style string.</param>
    /// <param name="when">The nullable condition in which the CSS style is added.</param>
    /// <returns>The <see cref="StyleBuilder"/> instance.</returns>
    public StyleBuilder AddRaw(string? style, bool? when) => when == true ? AddRaw(style) : this;

    /// <summary>
    /// Adds a conditional raw CSS style string to the builder.
    /// </summary>
    /// <param name="style">The CSS style string.</param>
    /// <param name="when">The condition function in which the CSS style is added.</param>
    /// <returns>The <see cref="StyleBuilder"/> instance.</returns>
    public StyleBuilder AddRaw(string? style, Func<bool>? when) => AddRaw(style, when is not null && when());

    /// <summary>
    /// Adds a conditional nested StyleBuilder to the builder.
    /// </summary>
    /// <param name="builder">The StyleBuilder to conditionally add.</param>
    /// <param name="when">The condition in which the StyleBuilder is added.</param>
    /// <returns>The <see cref="StyleBuilder"/> instance.</returns>
    public StyleBuilder AddRaw(StyleBuilder builder, bool when = true) => when ? AddRaw(builder.Build()) : this;

    /// <summary>
    /// Adds a conditional nested StyleBuilder to the builder.
    /// </summary>
    /// <param name="builder">The StyleBuilder to conditionally add.</param>
    /// <param name="when">The condition function in which the StyleBuilder is added.</param>
    /// <returns>The <see cref="StyleBuilder"/> instance.</returns>
    public StyleBuilder AddRaw(StyleBuilder builder, Func<bool>? when) => AddRaw(builder, when is not null && when());

    /// <summary>
    /// Adds a style from the "style" attribute in a dictionary if it exists.
    /// This is a null-safe operation.
    /// </summary>
    /// <param name="additionalAttributes">Additional attribute splat parameters.</param>
    /// <returns>The <see cref="StyleBuilder"/> instance.</returns>
    public StyleBuilder AddStyleFromAttributes(IReadOnlyDictionary<string, object>? additionalAttributes)
    {
        return additionalAttributes is null
            ? this
            : additionalAttributes.TryGetValue("style", out var result)
                ? AddRaw(result.ToString())
                : this;
    }

    /// <summary>
    /// Finalizes the completed CSS styles as a string.
    /// </summary>
    /// <returns>The string representation of the CSS styles.</returns>
    public string Build() => StringBuilderCache.GetStringAndRelease(EnsureCreated()).Trim();

    /// <summary>
    /// Finalizes the completed CSS styles as a string, or returns null if empty.
    /// </summary>
    /// <returns>The string representation of the CSS styles, or null if no styles were added.</returns>
    public string? BuildOrNull()
    {
        var result = Build();
        return string.IsNullOrWhiteSpace(result) ? null : result;
    }

    // ToString should only and always call Build to finalize the rendered string.
    /// <inheritdoc />
    public override string ToString() => Build();

    private StringBuilder EnsureCreated() => _stringBuilder ??= StringBuilderCache.Acquire();
}
