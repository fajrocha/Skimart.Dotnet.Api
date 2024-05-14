using Microsoft.Extensions.Options;
using Skimart.Application.Cache.Gateways;
using Skimart.Application.Configurations.Memory;
using Skimart.Application.Gateways.Persistence.Repositories.StoreProduct;
using Skimart.Application.Products.Queries.GetAllProducts;
using Skimart.Domain.Entities.Products;

namespace Skimart.Infrastructure.Cache;

public class ProductCacheRepository : IProductRepository
{
    private readonly IProductRepository _decorated;
    private readonly ICacheService _cacheService;
    private readonly CacheConfiguration _cacheConfiguration;

    public ProductCacheRepository(
        IProductRepository decorated, 
        ICacheService cacheService, 
        IOptions<CacheConfiguration> cache)
    {
        _decorated = decorated;
        _cacheService = cacheService;
        _cacheConfiguration = cache.Value;
    }
    
    public Task<List<Product>> GetEntitiesAsync()
    {
        return _decorated.GetEntitiesAsync();
    }

    public Task<Product> AddAsync(Product entity)
    {
        return _decorated.AddAsync(entity);
    }

    public void UpdateAsync(Product entity)
    {
        _decorated.UpdateAsync(entity);
    }

    public void Delete(Product entity)
    {
        _decorated.Delete(entity);
    }

    public async Task<Product?> GetEntityByIdAsync(int id)
    {
        var cacheKey = new CacheKeyBuilder()
            .WithPrefix(CacheKeyBuilder.GetById<Product>())
            .WithKey($"{nameof(id)}:{id}")
            .Build();

        return await _cacheService.GetOrCacheValueAsync(
            cacheKey,
            async () => await _decorated.GetEntityByIdAsync(id),
            TimeSpan.FromSeconds(_cacheConfiguration.ProductsTimeToLiveSecs));
    }

    public Task<int> SaveChangesAsync()
    {
        return _decorated.SaveChangesAsync();
    }

    public async Task<List<Product>> GetEntitiesAsync(GetAllProductsQuery productsQuery)
    {
        var cacheKey = new CacheKeyBuilder()
            .WithPrefix(CacheKeyBuilder.GetAll<Product>())
            .WithKey(productsQuery.ToString())
            .Build();

        return await _cacheService.GetOrCacheValueAsync(
            cacheKey,
            async () => await _decorated.GetEntitiesAsync(productsQuery),
            TimeSpan.FromSeconds(_cacheConfiguration.ProductsTimeToLiveSecs)) ?? new List<Product>();
    }

    public Task<int> CountAsync(GetAllProductsQuery productQuery)
    {
        return _decorated.CountAsync(productQuery);
    }
}