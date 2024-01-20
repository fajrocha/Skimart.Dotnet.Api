using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skimart.Application.Abstractions.Auth;
using Skimart.Application.Abstractions.Memory.Basket;
using Skimart.Application.Abstractions.Memory.Cache;
using Skimart.Application.Abstractions.Persistence.Migrators;
using Skimart.Application.Abstractions.Persistence.Repositories.StoreProduct;
using Skimart.Domain.Entities.Auth;
using Skimart.Infrastructure.Auth;
using Skimart.Infrastructure.Auth.Migrators;
using Skimart.Infrastructure.Auth.Services;
using Skimart.Infrastructure.Memory.Basket;
using Skimart.Infrastructure.Memory.Cache;
using Skimart.Infrastructure.Persistence.DbContexts;
using Skimart.Infrastructure.Persistence.Migrators.EntityFramework;
using Skimart.Infrastructure.Persistence.Repositories.StoreProduct;
using StackExchange.Redis;

namespace Skimart.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    private const string DefaultConnectionProp = "DefaultConnection";
    private const string IdentityConnectionProp = "IdentityConnection";
    
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPersistenceServices(configuration)
            .AddMemoryServices(configuration)
            .AddAuthServices(configuration);
        
        return services;
    }
    
    private static IServiceCollection AddMemoryServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            var options = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis") ?? string.Empty);
            return ConnectionMultiplexer.Connect(options);
        });
        services.AddSingleton<ICacheService, RedisCacheService>();
        services.AddScoped<IBasketRepository, BasketRedisRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString(DefaultConnectionProp) ?? string.Empty;
        services.AddDbContext<StoreContext>(o => o.UseSqlServer(connString));
        
        services.AddScoped<IDbMigrator, DbMigrator>();
        
        services.AddScoped<IProductRepository, EfProductRepository>();
        services.AddScoped<IProductBrandRepository, EfProductBrandRepository>();
        services.AddScoped<IProductTypeRepository, EfProductTypeRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString(IdentityConnectionProp) ?? string.Empty;
        services.AddDbContext<AppIdentityDbContext>(opt =>
        {
            opt.UseSqlServer(connString);
        });
        
        services
            .AddIdentityCore<AppUser>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddSignInManager<SignInManager<AppUser>>();
        
        services.AddScoped<IAuthMigrator, EfAuthMigrator>();
        services.AddScoped<IAuthService, EfIdentityAuthService>();
        services.AddSingleton<ITokenService, JwtTokenService>();
        
        services.AddAuthorizationCore();
        
        return services;
    }
}