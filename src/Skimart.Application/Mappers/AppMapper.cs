using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Skimart.Application.Mappers;

public static class AppMapper
{
    public static void BootstrapMapster(this IServiceCollection services)
    {
        var config = GetConfig();
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
    }

    public static IMapper GetMapper()
    {
        var config = GetConfig();

        return new Mapper(config);
    }

    private static TypeAdapterConfig GetConfig()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.RuleMap.Clear();
        config.Scan(Assembly.GetAssembly(typeof(MapsterMappingProfiles))!);

        return config;
    }
}