using Tablazor.Services;

namespace Tablazor.DemoSite.Services;

public class DemoBrandingProvider : IBrandingProvider
{
    /// <summary>
    /// The application name
    /// </summary>
    public string AppName => "Tablazor Demo Site";

    /// <summary>
    /// The optional application logo
    /// </summary>
    public string? AppLogoUrl => null;
}