using Domain.Entities.Product;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Skimart.Application.Abstractions.Memory.Cache;
using Skimart.Application.Abstractions.Persistence.Repositories.StoreProduct;
using Skimart.Application.Configurations.Memory;
using Skimart.Application.Helpers;

namespace Skimart.Application.Cases.Products.Queries.GetAllProductTypes;

public class GetAllTypesHandler : IRequestHandler<GetAllTypesQuery, IReadOnlyList<ProductType>>
{
    private readonly ILogger<GetAllTypesHandler> _logger;
    private readonly IProductTypeRepository _typesRepos;
    private readonly ICacheHandler _cacheHandler;
    private readonly CacheConfig _cacheConfig;

    public GetAllTypesHandler(
        ILogger<GetAllTypesHandler> logger, 
        IOptions<CacheConfig> opts, 
        IProductTypeRepository typesRepos,
        ICacheHandler cacheHandler)
    {
        _logger = logger;
        _cacheConfig = opts.Value;
        _typesRepos = typesRepos;
        _cacheHandler = cacheHandler;
    }
    
    public async Task<IReadOnlyList<ProductType>> Handle(GetAllTypesQuery query, CancellationToken cancellationToken)
    {
        var cachedResponse = await _cacheHandler.GetCachedResponseAsync<IReadOnlyList<ProductType>>(query.RequestDto);
        if (cachedResponse is not null)
        {
            return cachedResponse;
        }
        
        var productTypes = await _typesRepos.GetEntitiesAsync();
        var timeToLive = TimeSpan.FromSeconds(_cacheConfig.TypesTimeToLive);
        
        await _cacheHandler.CacheResponseAsync(query.RequestDto, productTypes, timeToLive);
        
        return productTypes;
    }
}