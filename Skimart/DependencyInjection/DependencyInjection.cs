using Skimart.Configurations.Memory;

namespace Skimart.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddConfigurationsBinding();
        
        return services;
    }
    
    private static IServiceCollection AddConfigurationsBinding(this IServiceCollection services)
    {
        services.ConfigureOptions<CacheConfigSetup>();

        return services;
    }
}