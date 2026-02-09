using Tablazor.Services;

namespace Microsoft.Extensions.DependencyInjection;

/// <exclude />
/// <summary>
/// Extension class to register Tablazor services to Dependency Injection
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Tablazor services
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> reference</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddTablazor(this IServiceCollection services)
    {
        services.AddScoped<TabPageHeaderService>();
        services.AddScoped<TabToastService>();

        return services;
    }
}