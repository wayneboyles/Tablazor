using Tablazor.Attributes;

namespace System;

/// <exclude />
/// <summary>
/// Extension methods for retrieving custom properties from
/// an <see cref="Enum"/>
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the CSS variable name for a TablerColor enum value
    /// </summary>
    /// <param name="enum">The TablerColor enum value</param>
    /// <returns>The CSS variable name (e.g., "--tblr-primary")</returns>
    public static string GetCssVariable(this Enum @enum)
    {
        var fieldInfo = @enum.GetType().GetField(@enum.ToString());

        if (fieldInfo == null)
        {
            return string.Empty;
        }

        var attribute = (CssVariableNameAttribute)Attribute.GetCustomAttribute(
            fieldInfo,
            typeof(CssVariableNameAttribute)
        )!;

        return attribute?.Variable ?? string.Empty;
    }

    /// <summary>
    /// Gets the CSS variable reference for use in styles (e.g., "var(--tblr-primary)")
    /// </summary>
    /// <param name="enum">The TablerColor enum value</param>
    /// <returns>The CSS variable reference string</returns>
    public static string GetCssVariableReference(this Enum @enum)
    {
        var variable = @enum.GetCssVariable();
        return !string.IsNullOrEmpty(variable) ? $"var({variable})" : string.Empty;
    }

    /// <summary>
    /// Gets the CSS class name for a TablerColor enum value
    /// </summary>
    /// <param name="enum">The TablerColor enum value</param>
    /// <returns>The CSS class name (e.g., "primary", "success")</returns>
    public static string GetCssClassName(this Enum @enum)
    {
        var fieldInfo = @enum.GetType().GetField(@enum.ToString());

        if (fieldInfo == null)
        {
            return string.Empty;

        }

        var attribute = (CssClassNameAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(CssClassNameAttribute))!;

        return attribute?.ClassName ?? string.Empty;
    }

    /// <summary>
    /// Gets the CSS class name with a prefix (e.g., "bg-primary", "text-success")
    /// </summary>
    /// <param name="enum">The TablerColor enum value</param>
    /// <param name="prefix">The prefix to add before the class name (e.g., "bg", "text", "btn")</param>
    /// <returns>The prefixed CSS class name</returns>
    public static string GetCssClassNameWithPrefix(this Enum @enum, string prefix)
    {
        var className = @enum.GetCssClassName();
        return !string.IsNullOrEmpty(className) ? $"{prefix}-{className}" : string.Empty;
    }
}