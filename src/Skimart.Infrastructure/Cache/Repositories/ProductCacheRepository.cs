using Microsoft.Extensions.Options;
using Skimart.Application.Products.Gateways;
using Skimart.Application.Products.Queries.GetAllProducts;
using Skimart.Domain.Entities.Products;
using Skimart.Infrastructure.Cache.Abstractions;
using Skimart.Infrastructure.Cache.Configurations;
using Skimart.Infrastructure.Cache.Helpers;

namespace Skimart.Infrastructure.Cache.Repositories;

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
        return await _cacheService.GetOrCacheValueAsync(
            key => key.WithPrefix(CacheKeyBuilder.KeyGetById<Product>())
                .WithName(CacheKeyBuilder.KeyGetById(id)),
            async () => await _decorated.GetEntityByIdAsync(id),
            TimeSpan.FromSeconds(_cacheConfiguration.ProductsTimeToLiveSecs));
    }

    public Task<int> SaveChangesAsync()
    {
        return _decorated.SaveChangesAsync();
    }

    public async Task<List<Product>> GetEntitiesAsync(GetAllProductsQuery productsQuery)
    {
        return await _cacheService.GetOrCacheValueAsync(
            key => key.WithPrefix(CacheKeyBuilder.KeyGetAll<Product>())
                .WithName(productsQuery.ToString()),
            async () => await _decorated.GetEntitiesAsync(productsQuery),
            TimeSpan.FromSeconds(_cacheConfiguration.ProductsTimeToLiveSecs)) ?? new List<Product>();
    }

    public Task<int> CountAsync(GetAllProductsQuery productQuery)
    {
        return _decorated.CountAsync(productQuery);
    }
}