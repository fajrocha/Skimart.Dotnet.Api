using Microsoft.AspNetCore.Authorization;
using Skimart.Application.Identity.Gateways;
using Skimart.Basket.Configurations;
using Skimart.Cache.Configurations;
using Skimart.Identity;
using Skimart.Identity.Configurations;
using Skimart.Payment.Configurations;

namespace Skimart.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddConfigurationsBinding();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        return services;
    }
    
    private static IServiceCollection AddConfigurationsBinding(this IServiceCollection services)
    {
        services.ConfigureOptions<CacheConfigSetup>()
            .ConfigureOptions<BasketConfigSetup>()
            .ConfigureOptions<TokenConfigSetup>()
            .ConfigureOptions<PaymentConfigSetup>();

        return services;
    }
}