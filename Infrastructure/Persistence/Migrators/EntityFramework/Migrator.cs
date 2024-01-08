using Application.Abstractions.Persistence.Migrators;
using Infrastructure.Persistence.DataSeed;
using Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Migrators.EntityFramework;

public class Migrator : IMigrator
{
    private readonly ILogger<StoreContext> _logger;
    private readonly StoreContext _storeContext;

    public Migrator(ILogger<StoreContext> logger, StoreContext storeContext)
    {
        _logger = logger;
        _storeContext = storeContext;
    }

    public async Task MigrateAsync()
    {
        await _storeContext.Database.MigrateAsync();
    }

    public async Task SeedDataAsync()
    {
        await DataSeeder.SeedAsync(_storeContext, _logger);
    }
}