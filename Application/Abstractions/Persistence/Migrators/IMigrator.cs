namespace Application.Abstractions.Persistence.Migrators;

public interface IMigrator
{
    Task MigrateAsync();
    Task SeedDataAsync();
}