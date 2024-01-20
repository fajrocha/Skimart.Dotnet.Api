using Microsoft.Extensions.Hosting;
using Serilog;

namespace Skimart.Infrastructure.Logging;

public static class AppLogger
{
    public static void BootstrapLogger(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, log) =>
        {
            log.ReadFrom.Configuration(context.Configuration);
        });
    }
}