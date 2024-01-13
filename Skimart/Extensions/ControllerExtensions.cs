using System.Text.Json.Serialization;

namespace Presentation.Extensions;

public static class ControllerExtensions
{
    public static void AddAppControllers(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(opts =>
                opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
    }
}