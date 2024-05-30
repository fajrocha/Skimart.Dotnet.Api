using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Skimart.Application.Shared.Gateways;
using Skimart.Infrastructure.Store.DataSeed;

namespace Skimart.Infrastructure.Store.Migrators.EntityFramework;

public class DbMigrator : IDbMigrator
{
    private readonly ILogger<StoreContext> _logger;
    private readonly StoreContext _storeContext;

    public DbMigrator(ILogger<StoreContext> logger, StoreContext storeContext)
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