using Microsoft.Extensions.DependencyInjection;
using Skimart.Application.Abstractions.Memory.Cache;
using Skimart.Application.Helpers;
using Skimart.Application.Mappers;

namespace Skimart.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.BootstrapMapster();

        services.AddScoped<ICacheHandler, CacheHandler>();
        
        return services;
    }
}