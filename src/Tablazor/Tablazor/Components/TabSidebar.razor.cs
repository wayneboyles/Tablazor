using Microsoft.AspNetCore.Components;

namespace Tablazor.Components;

/// <summary>
/// A vertical sidebar navigation that renders as <c>&lt;aside class="navbar navbar-vertical navbar-expand-lg"&gt;</c>.
/// Supports a brand/logo area and navigation link content.
/// </summary>
/// <remarks>
/// <para>
/// Use <see cref="TabSidebar"/> inside a <see cref="TabLayout"/> for the vertical sidebar layout mode.
/// Place the sidebar before the <see cref="TabPageWrapper"/>.
/// </para>
/// <example>
/// <code>
/// &lt;TabLayout&gt;
///     &lt;TabSidebar Dark="true"&gt;
///         &lt;Brand&gt;
///             &lt;a href="/" class="navbar-brand"&gt;My App&lt;/a&gt;
///         &lt;/Brand&gt;
///         &lt;NavContent&gt;
///             &lt;ul class="navbar-nav pt-lg-3"&gt;
///                 &lt;li class="nav-item"&gt;&lt;a class="nav-link" href="/"&gt;Home&lt;/a&gt;&lt;/li&gt;
///             &lt;/ul&gt;
///         &lt;/NavContent&gt;
///     &lt;/TabSidebar&gt;
///     &lt;TabPageWrapper&gt;
///         &lt;TabPageBody&gt;...&lt;/TabPageBody&gt;
///     &lt;/TabPageWrapper&gt;
/// &lt;/TabLayout&gt;
/// </code>
/// </example>
/// </remarks>
public partial class TabSidebar : TabBaseComponent
{
    private string _sidebarId = string.Empty;

    /// <summary>
    /// Gets or sets the brand or logo content rendered in the <c>navbar-brand</c> area.
    /// </summary>
    [Parameter]
    public RenderFragment? Brand { get; set; }

    /// <summary>
    /// Gets or sets the navigation content rendered inside the collapsible sidebar area.
    /// Typically contains a <c>&lt;ul class="navbar-nav pt-lg-3"&gt;</c> with navigation items.
    /// </summary>
    [Parameter]
    public RenderFragment? NavContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the dark theme variant is applied.
    /// When <c>true</c>, adds the <c>navbar-dark</c> CSS class.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    [Parameter]
    public bool Dark { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the condensed (narrow) sidebar variant is used.
    /// When <c>true</c>, adds the <c>navbar-vertical-sm</c> CSS class for a narrower sidebar.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    [Parameter]
    public bool Condensed { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _sidebarId = GetId() + "-menu";
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("navbar navbar-vertical navbar-expand-lg")
            .AddClass("navbar-dark", Dark)
            .AddClass("navbar-vertical-sm", Condensed)
            .AddClass(CssClass)
            .Build();
    }
}
