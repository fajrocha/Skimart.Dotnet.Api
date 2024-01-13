﻿using Skimart.Application.Abstractions.Persistence.Migrators;

namespace Skimart.Extensions.Migrations;

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
            await MigrateAndSeedData(logger, storeMigrator, "Store Context");
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "Error occurred while doing the DB migration.");
        }
    }

    private static async Task MigrateAndSeedData(ILogger logger, IDbMigrator dbMigrator, string contextName)
    {
        logger?.LogInformation("Starting doing the {contextName} migration...", contextName);
        await dbMigrator.MigrateAsync();
        await dbMigrator.SeedDataAsync();
    }
}