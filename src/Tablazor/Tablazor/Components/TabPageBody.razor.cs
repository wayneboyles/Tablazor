using Microsoft.AspNetCore.Components;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// The main content area of the page, rendered as <c>&lt;div class="page-body"&gt;</c>.
/// Optionally wraps its content in a responsive container element.
/// </summary>
/// <remarks>
/// <para>
/// When <see cref="ContainerSize"/> is set, the child content is wrapped in a
/// <c>container-{size}</c> div. When left as <c>null</c>, the content is rendered
/// without any container wrapper, allowing custom container control.
/// </para>
/// <example>
/// <code>
/// &lt;TabPageBody ContainerSize="ContainerSize.ExtraLarge"&gt;
///     &lt;div class="row"&gt;
///         &lt;!-- page content --&gt;
///     &lt;/div&gt;
/// &lt;/TabPageBody&gt;
/// </code>
/// </example>
/// </remarks>
public partial class TabPageBody : TabBaseComponent
{
    /// <summary>
    /// Gets or sets the child content rendered inside the page body.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the container size used to wrap the child content.
    /// When <c>null</c>, the child content is rendered without a container wrapper.
    /// </summary>
    /// <value>The default value is <c>null</c> (no container wrapper).</value>
    [Parameter]
    public ContainerSize? ContainerSize { get; set; }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("page-body")
            .AddClass(CssClass)
            .Build();
    }
}
