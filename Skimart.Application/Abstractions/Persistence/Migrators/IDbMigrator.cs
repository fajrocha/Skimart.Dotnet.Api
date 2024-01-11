namespace Skimart.Application.Abstractions.Persistence.Migrators;

public interface IDbMigrator
{
    Task MigrateAsync();
    Task SeedDataAsync();
}