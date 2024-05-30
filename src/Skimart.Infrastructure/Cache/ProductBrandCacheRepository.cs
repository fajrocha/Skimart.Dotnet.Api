using Microsoft.Extensions.Options;
using Skimart.Application.Cache;
using Skimart.Application.Cache.Configurations;
using Skimart.Application.Cache.Gateways;
using Skimart.Application.Products.Gateways;
using Skimart.Domain.Entities.Products;

namespace Skimart.Infrastructure.Cache;

public class ProductBrandCacheRepository : IProductBrandRepository
{
    private readonly IProductBrandRepository _decorated;
    private readonly ICacheService _cacheService;
    private readonly CacheConfiguration _cacheConfiguration;

    public ProductBrandCacheRepository(
        IProductBrandRepository decorated, 
        ICacheService cacheService,
        IOptions<CacheConfiguration> cacheConfiguration)
    {
        _decorated = decorated;
        _cacheService = cacheService;
        _cacheConfiguration = cacheConfiguration.Value;
    }
    
    public async Task<List<ProductBrand>> GetEntitiesAsync()
    {
        return await _cacheService.GetOrCacheValueAsync(
            key => key.WithName(CacheKeyBuilder.KeyGetAll<ProductBrand>()),
            async () => await _decorated.GetEntitiesAsync(),
            TimeSpan.FromSeconds(_cacheConfiguration.TypesTimeToLiveSecs)) ?? new List<ProductBrand>();
    }

    public Task<ProductBrand> AddAsync(ProductBrand entity)
    {
        return _decorated.AddAsync(entity);
    }

    public void UpdateAsync(ProductBrand entity)
    {
        _decorated.UpdateAsync(entity);
    }

    public void Delete(ProductBrand entity)
    {
        _decorated.UpdateAsync(entity);
    }

    public Task<ProductBrand?> GetEntityByIdAsync(int id)
    {
        return _decorated.GetEntityByIdAsync(id);
    }

    public Task<int> SaveChangesAsync()
    {
        return _decorated.SaveChangesAsync();
    }
}