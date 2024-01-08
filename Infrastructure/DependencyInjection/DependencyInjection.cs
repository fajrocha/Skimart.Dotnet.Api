using Application.Abstractions.Persistence.Migrators;
using Application.Abstractions.Persistence.Repositories.StoreProduct;
using Infrastructure.Persistence.DbContexts;
using Infrastructure.Persistence.Migrators.EntityFramework;
using Infrastructure.Persistence.Repositories.StoreProduct;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    private const string DefaultConnectionProp = "DefaultConnection";
    
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPersistenceServices(configuration);
        
        return services;
    }
    
    private static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString(DefaultConnectionProp) ?? string.Empty;
        services.AddDbContext<StoreContext>(o => o.UseSqlServer(connString));
        
        services.AddScoped<IMigrator, Migrator>();
        
        services.AddScoped<IProductRepository, EfProductRepository>();
        services.AddScoped<IProductBrandRepository, EfProductBrandRepository>();
        services.AddScoped<IProductTypeRepository, EfProductTypeRepository>();
        
        return services;
    }
}