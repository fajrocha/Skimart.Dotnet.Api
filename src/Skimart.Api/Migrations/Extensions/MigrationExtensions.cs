using Skimart.Application.Identity.Gateways;
using Skimart.Application.Shared.Gateways;

namespace Skimart.Migrations.Extensions;

public static class MigrationExtensions
{
    public static async Task MigrateDbs(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetService<ILoggerFactory>();
        var logger = loggerFactory?.CreateLogger<Program>();
        try
        {
            var storeMigrator = scope.ServiceProvider.GetRequiredService<IDbMigrator>();
            await MigrateAndSeedData(logger!, storeMigrator, "Store Context");
            var authMigrator = scope.ServiceProvider.GetRequiredService<IAuthMigrator>();
            await MigrateAndSeedData(logger!, authMigrator, "User Context");
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "Error occurred while doing the DB migration.");
        }
    }

    private static async Task MigrateAndSeedData(ILogger logger, IDbMigrator dbMigrator, string contextName)
    {
        logger.LogInformation("Starting doing the {contextName} migration...", contextName);
        await dbMigrator.MigrateAsync();
        await dbMigrator.SeedDataAsync();
    }
}