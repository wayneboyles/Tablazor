namespace Tablazor.Template.Services;

public sealed class DefaultBrandingProvider : IBrandingProvider
{
    public string ApplicationName => "My Application";

    public string? LogoUrl => null;
}