using System.Reflection;

using Tablazor.Attributes;
using Tablazor.Enums;

namespace Tablazor.Models;

/// <summary>
/// Internal resolved representation of a single form field.
/// Built once during <c>OnInitialized</c> / <c>OnParametersSet</c> by reflecting on
/// the model type and merging attribute-level and form-level configuration.
/// </summary>
internal sealed class FieldDescriptor
{
    // ── Source ────────────────────────────────────────────────────────────────

    /// <summary>The reflected public instance property on the model type.</summary>
    public required PropertyInfo Property { get; init; }

    // ── Raw attributes ────────────────────────────────────────────────────────

    /// <summary>The <see cref="TabFieldAttribute"/> on the property, or <c>null</c>.</summary>
    public TabFieldAttribute? FieldAttr { get; init; }

    /// <summary>The <see cref="SameRowAttribute"/> on the property, or <c>null</c>.</summary>
    public SameRowAttribute? SameRowAttr { get; init; }

    /// <summary>The <see cref="TabSectionAttribute"/> on the property, or <c>null</c>.</summary>
    public TabSectionAttribute? SectionAttr { get; init; }

    // ── Resolved config ───────────────────────────────────────────────────────

    /// <summary>Final display label text (after attribute / name fallback resolution).</summary>
    public required string Label { get; init; }

    /// <summary>Effective label position after merging field-level and form-level settings.</summary>
    public LabelPosition EffectiveLabelPosition { get; set; }

    /// <summary>Effective field/input type after auto-detection or explicit override.</summary>
    public TabFieldType EffectiveFieldType { get; set; }

    // ── Type metadata ─────────────────────────────────────────────────────────

    /// <summary><c>true</c> when the property carries a <c>[Required]</c> attribute.</summary>
    public bool IsRequired { get; init; }

    /// <summary>
    /// <c>true</c> when the property type is nullable
    /// (e.g. <c>int?</c>, <c>DateTime?</c>, <c>MyEnum?</c>).
    /// </summary>
    public bool IsNullable { get; init; }

    /// <summary>
    /// The non-nullable underlying CLR type
    /// (e.g. <c>int</c> for <c>int?</c>, or the property type itself for non-nullable).
    /// </summary>
    public required Type UnderlyingType { get; init; }

    // ── Sort ──────────────────────────────────────────────────────────────────

    /// <summary>Resolved display order used when sorting fields.</summary>
    public int Order { get; init; }

    // ── Computed helpers ──────────────────────────────────────────────────────

    /// <summary>A stable, HTML-safe <c>id</c> derived from the property name.</summary>
    public string FieldId => $"dynform-{Property.Name.ToLowerInvariant()}";

    /// <summary><c>true</c> when a left icon is configured via <see cref="TabFieldAttribute.IconLeft"/>.</summary>
    public bool HasIconLeft => !string.IsNullOrWhiteSpace(FieldAttr?.IconLeft);

    /// <summary><c>true</c> when a right icon is configured via <see cref="TabFieldAttribute.IconRight"/>.</summary>
    public bool HasIconRight => !string.IsNullOrWhiteSpace(FieldAttr?.IconRight);
}