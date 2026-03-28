using Microsoft.AspNetCore.Components;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// A page footer element rendered as <c>&lt;footer class="footer d-print-none"&gt;</c>.
/// Wraps its content in a responsive container element.
/// </summary>
/// <remarks>
/// <para>
/// Place <see cref="TabFooter"/> at the bottom of a <see cref="TabPageWrapper"/>, after the
/// <see cref="TabPageBody"/>. Use <see cref="Transparent"/> for a footer that blends with
/// the page background.
/// </para>
/// <example>
/// <code>
/// &lt;TabFooter Transparent="true" ContainerSize="ContainerSize.ExtraLarge"&gt;
///     &lt;p class="text-muted"&gt;Copyright &amp;copy; 2024 My App&lt;/p&gt;
/// &lt;/TabFooter&gt;
/// </code>
/// </example>
/// </remarks>
public partial class TabFooter : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the child content rendered inside the footer container.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the transparent footer variant is applied.
    /// When <c>true</c>, adds the <c>footer-transparent</c> CSS class.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    [Parameter]
    public bool Transparent { get; set; }

    /// <summary>
    /// Gets or sets the container size used to wrap the footer content.
    /// </summary>
    /// <value>The default value is <see cref="ContainerSize.ExtraLarge"/>.</value>
    [Parameter]
    public ContainerSize ContainerSize { get; set; } = ContainerSize.ExtraLarge;

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("footer d-print-none")
            .AddClass("footer-transparent", Transparent)
            .AddClass(CssClass)
            .Build();
    }
}
