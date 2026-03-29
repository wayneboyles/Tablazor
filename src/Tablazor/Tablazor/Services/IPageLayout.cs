namespace Tablazor.Services;

/// <summary>
/// Provides access to set the page title and
/// subtitle on a per-page basis
/// </summary>
public interface IPageLayout
{
    /// <summary>
    /// The page title
    /// </summary>
    string Title { get; set; }
    
    /// <summary>
    /// The optional page subtitle
    /// </summary>
    string? SubTitle { get; set; }
}

/// <summary>
/// Provides access to set the page title and
/// subtitle on a per-page basis
/// </summary>
public class DefaultPageLayout : IPageLayout
{
    /// <summary>
    /// The page title
    /// </summary>
    public string Title { get; set; } = "Page Title";

    /// <summary>
    /// The optional page subtitle
    /// </summary>
    public string? SubTitle { get; set; } = null;
}