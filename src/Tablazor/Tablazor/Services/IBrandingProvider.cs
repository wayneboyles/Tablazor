namespace Tablazor.Services;

/// <summary>
/// Branding provider to provide a consistent application
/// name and logo
/// </summary>
public interface IBrandingProvider
{
    /// <summary>
    /// The application name
    /// </summary>
    string AppName { get; }
    
    /// <summary>
    /// The optional application logo
    /// </summary>
    string? AppLogoUrl { get; }
}

/// <summary>
/// Default implementation of the branding provider
/// </summary>
public class DefaultBrandingProvider : IBrandingProvider
{
    /// <summary>
    /// The application name
    /// </summary>
    public string AppName => "My Tablazor Application";

    /// <summary>
    /// The optional application logo
    /// </summary>
    public string? AppLogoUrl => null;
}