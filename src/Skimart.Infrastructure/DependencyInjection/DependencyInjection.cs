using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skimart.Application.Basket.Gateways;
using Skimart.Application.Cache.Gateways;
using Skimart.Application.Identity.Gateways;
using Skimart.Application.Orders.Gateways;
using Skimart.Application.Payment.Gateways;
using Skimart.Application.Products.Gateways;
using Skimart.Application.Shared.Gateways;
using Skimart.Domain.Entities.Auth;
using Skimart.Infrastructure.Auth;
using Skimart.Infrastructure.Basket;
using Skimart.Infrastructure.Cache;
using Skimart.Infrastructure.Identity.Migrators;
using Skimart.Infrastructure.Identity.Services;
using Skimart.Infrastructure.Orders.Repositories;
using Skimart.Infrastructure.Payment.Services;
using Skimart.Infrastructure.Products.Repositories;
using Skimart.Infrastructure.Shared;
using Skimart.Infrastructure.Shared.Migrators.EntityFramework;
using Skimart.Infrastructure.Shared.Repositories;
using Skimart.Infrastructure.Store;
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
            .AddAuthServices(configuration)
            .AddPaymentServices();
        
        return services;
    }
    
    private static IServiceCollection AddMemoryServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            var options = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis") ?? string.Empty);
            return ConnectionMultiplexer.Connect(options);
        });
        services.AddStackExchangeRedisCache(opts =>
            opts.Configuration = configuration.GetConnectionString("Redis"));
        services.AddSingleton<ICacheService, RedisCacheService>();
        services.AddScoped<IBasketRepository, BasketRedisRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString(DefaultConnectionProp) ?? string.Empty;
        services.AddDbContext<StoreContext>(o => o.UseNpgsql(connString));
        
        services.AddScoped<IDbMigrator, DbMigrator>();
        
        services.AddScoped<IUnitOfWork, StoreUnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.Decorate<IProductRepository, ProductCacheRepository>();
        services.AddScoped<IProductBrandRepository, ProductBrandRepository>();
        services.Decorate<IProductBrandRepository, ProductBrandCacheRepository>();
        services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
        services.Decorate<IProductTypeRepository, ProductTypeCacheRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IDeliveryMethodRepository, DeliveryMethodRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString(IdentityConnectionProp) ?? string.Empty;
        services.AddDbContext<AppIdentityDbContext>(opt =>
        {
            opt.UseNpgsql(connString);
        });
        
        services
            .AddIdentityCore<AppUser>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddSignInManager<SignInManager<AppUser>>();
        
        services.AddScoped<IAuthMigrator, EfAuthMigrator>();
        services.AddScoped<IAuthService, IdentityAuthService>();
        services.AddSingleton<ITokenService, JwtTokenService>();
        
        services.AddAuthorizationCore();
        
        return services;
    }

    private static IServiceCollection AddPaymentServices(this IServiceCollection services)
    {
        services.AddScoped<IPaymentGateway, StripePaymentService>();
        
        return services;
    }
}