using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// A wrapper element that contains the main page content area, including the page body and footer.
/// Renders as <c>&lt;div class="page-wrapper"&gt;</c>.
/// </summary>
/// <remarks>
/// <para>
/// Place <see cref="TabPageWrapper"/> after the <see cref="TabNavbar"/> or <see cref="TabSidebar"/>
/// inside a <see cref="TabLayout"/>. It contains the <see cref="TabPageBody"/> and optionally
/// a <see cref="TabFooter"/>.
/// </para>
/// <example>
/// <code>
/// &lt;TabLayout&gt;
///     &lt;TabNavbar&gt;...&lt;/TabNavbar&gt;
///     &lt;TabPageWrapper&gt;
///         &lt;TabPageBody ContainerSize="ContainerSize.ExtraLarge"&gt;
///             &lt;!-- page content --&gt;
///         &lt;/TabPageBody&gt;
///         &lt;TabFooter&gt;...&lt;/TabFooter&gt;
///     &lt;/TabPageWrapper&gt;
/// &lt;/TabLayout&gt;
/// </code>
/// </example>
/// </remarks>
public partial class TabPageWrapper : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the child content rendered inside the page wrapper.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("page-wrapper")
            .AddClass(CssClass)
            .Build();
    }
}
