using Microsoft.AspNetCore.Components;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// A horizontal top navigation bar that renders as <c>&lt;header class="navbar navbar-expand-md d-print-none"&gt;</c>.
/// Supports a brand/logo area, main navigation links, and right-side items such as user menus or notifications.
/// </summary>
/// <remarks>
/// <para>
/// Use <see cref="TabNavbar"/> inside a <see cref="TabLayout"/> for the horizontal layout mode,
/// or as a secondary header bar in a vertical sidebar layout (inside <see cref="TabPageWrapper"/>).
/// </para>
/// <example>
/// <code>
/// &lt;TabNavbar Dark="true"&gt;
///     &lt;Brand&gt;
///         &lt;a href="/" class="navbar-brand"&gt;My App&lt;/a&gt;
///     &lt;/Brand&gt;
///     &lt;NavContent&gt;
///         &lt;ul class="navbar-nav"&gt;
///             &lt;li class="nav-item"&gt;&lt;a class="nav-link" href="/"&gt;Home&lt;/a&gt;&lt;/li&gt;
///         &lt;/ul&gt;
///     &lt;/NavContent&gt;
/// &lt;/TabNavbar&gt;
/// </code>
/// </example>
/// </remarks>
public partial class TabNavbar : TabBaseComponent
{
    private string _navId = string.Empty;

    /// <summary>
    /// Gets or sets the brand or logo content rendered in the <c>navbar-brand</c> area.
    /// </summary>
    [Parameter]
    public RenderFragment? Brand { get; set; }

    /// <summary>
    /// Gets or sets the main navigation content rendered inside the collapsible
    /// <c>navbar-collapse</c> area.
    /// </summary>
    [Parameter]
    public RenderFragment? NavContent { get; set; }

    /// <summary>
    /// Gets or sets the content rendered on the right side of the navbar
    /// (e.g., user menu, notifications). Rendered with <c>navbar-nav flex-row order-md-last</c>.
    /// </summary>
    [Parameter]
    public RenderFragment? RightContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the dark theme variant is applied.
    /// When <c>true</c>, adds the <c>navbar-dark</c> CSS class.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    [Parameter]
    public bool Dark { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the transparent variant is applied.
    /// When <c>true</c>, adds the <c>navbar-transparent</c> CSS class.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    [Parameter]
    public bool Transparent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the navbar overlaps page content.
    /// When <c>true</c>, adds the <c>navbar-overlap</c> CSS class, which is typically
    /// used with colored hero headers.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    [Parameter]
    public bool Overlap { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the navbar is sticky at the top of the viewport.
    /// When <c>true</c>, adds the <c>sticky-top</c> CSS class.
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    [Parameter]
    public bool Sticky { get; set; }

    /// <summary>
    /// Gets or sets the container width used to wrap the navbar content.
    /// </summary>
    /// <value>The default value is <see cref="ContainerSize.ExtraLarge"/>.</value>
    [Parameter]
    public ContainerSize ContainerSize { get; set; } = ContainerSize.ExtraLarge;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _navId = GetId() + "-menu";
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("navbar navbar-expand-md d-print-none")
            .AddClass("navbar-dark", Dark)
            .AddClass("navbar-transparent", Transparent)
            .AddClass("navbar-overlap", Overlap)
            .AddClass("sticky-top", Sticky)
            .AddClass(CssClass)
            .Build();
    }
}
