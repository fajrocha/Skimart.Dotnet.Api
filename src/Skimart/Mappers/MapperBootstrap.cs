using System.Reflection;
using Mapster;
using MapsterMapper;

namespace Skimart.Mappers;

public static class MapperBootstrap
{
    public static void BootstrapMapper(this IServiceCollection services)
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