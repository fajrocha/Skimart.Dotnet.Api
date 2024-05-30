namespace Skimart.Application.Shared.Gateways;

public interface IDbMigrator
{
    Task MigrateAsync();
    Task SeedDataAsync();
}