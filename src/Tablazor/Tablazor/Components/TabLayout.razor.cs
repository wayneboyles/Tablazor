using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// The outermost page wrapper for a Tabler layout. Renders a <c>&lt;div class="page"&gt;</c>
/// that contains the entire page structure including the navbar or sidebar and the page wrapper.
/// </summary>
/// <remarks>
/// <para>
/// Use <see cref="TabLayout"/> as the root element when building a Tabler-styled page.
/// Place a <see cref="TabNavbar"/> or <see cref="TabSidebar"/> directly inside it, followed
/// by a <see cref="TabPageWrapper"/>.
/// </para>
/// <example>
/// <code>
/// &lt;TabLayout&gt;
///     &lt;TabNavbar&gt;...&lt;/TabNavbar&gt;
///     &lt;TabPageWrapper&gt;
///         &lt;TabPageBody&gt;...&lt;/TabPageBody&gt;
///         &lt;TabFooter&gt;...&lt;/TabFooter&gt;
///     &lt;/TabPageWrapper&gt;
/// &lt;/TabLayout&gt;
/// </code>
/// </example>
/// </remarks>
public partial class TabLayout : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the child content rendered inside the page wrapper.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("page")
            .AddClass(CssClass)
            .Build();
    }
}
