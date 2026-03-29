namespace Tablazor.Template.Services;

public interface IBrandingProvider
{
    public string ApplicationName { get; }
    
    public string? LogoUrl { get; }
}