using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Skimart.Application.Cache;
using Skimart.Application.Cache.Configurations;
using Skimart.Application.Cache.Gateways;
using Skimart.Application.Products.Gateways;
using Skimart.Domain.Entities.Products;

namespace Skimart.Infrastructure.Cache;

public class ProductTypeCacheRepository : IProductTypeRepository
{
    private readonly IProductTypeRepository _decorated;
    private readonly ICacheService _cacheService;
    private readonly CacheConfiguration _cacheConfiguration;

    public ProductTypeCacheRepository(
        IProductTypeRepository decorated, 
        ICacheService cacheService,
        IOptions<CacheConfiguration> cacheConfiguration)
    {
        _decorated = decorated;
        _cacheService = cacheService;
        _cacheConfiguration = cacheConfiguration.Value;
    }
    
    public async Task<List<ProductType>> GetEntitiesAsync()
    {
        return await _cacheService.GetOrCacheValueAsync(
            key => key.WithName(CacheKeyBuilder.KeyGetAll<ProductType>()),
            async () => await _decorated.GetEntitiesAsync(),
            TimeSpan.FromSeconds(_cacheConfiguration.TypesTimeToLiveSecs)) ?? new List<ProductType>();
    }

    public Task<ProductType> AddAsync(ProductType entity)
    {
        return _decorated.AddAsync(entity);
    }

    public void UpdateAsync(ProductType entity)
    {
        _decorated.UpdateAsync(entity);
    }

    public void Delete(ProductType entity)
    {
        _decorated.UpdateAsync(entity);
    }

    public Task<ProductType?> GetEntityByIdAsync(int id)
    {
        return _decorated.GetEntityByIdAsync(id);
    }

    public Task<int> SaveChangesAsync()
    {
        return _decorated.SaveChangesAsync();
    }
}