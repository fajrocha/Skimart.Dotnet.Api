using System.Text.Json.Serialization;

namespace Skimart.Extensions;

public static class ControllerExtensions
{
    public static void AddAppControllers(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(opts =>
                opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
    }
}