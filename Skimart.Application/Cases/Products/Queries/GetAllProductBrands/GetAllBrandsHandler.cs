using Domain.Entities.Product;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Skimart.Application.Abstractions.Memory.Cache;
using Skimart.Application.Abstractions.Persistence.Repositories.StoreProduct;
using Skimart.Application.Cases.Products.Dtos;
using Skimart.Application.Cases.Shared.Vms;
using Skimart.Application.Configurations.Memory;
using Skimart.Application.Helpers;

namespace Skimart.Application.Cases.Products.Queries.GetAllProductBrands;

public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, IReadOnlyList<ProductBrand>>
{
    private readonly ILogger<GetAllBrandsHandler> _logger;
    private readonly IProductBrandRepository _brandRepos;
    private readonly ICacheHandler _cacheHandler;
    private readonly CacheConfig _cacheConfig;

    public GetAllBrandsHandler(
        ILogger<GetAllBrandsHandler> logger,
        IOptions<CacheConfig> opts,
        IProductBrandRepository brandRepos,
        ICacheHandler cacheHandler)
    {
        _logger = logger;
        _cacheConfig = opts.Value;
        _brandRepos = brandRepos;
        _cacheHandler = cacheHandler;
    }
    
    public async Task<IReadOnlyList<ProductBrand>> Handle(GetAllBrandsQuery query, CancellationToken cancellationToken)
    {
        var cachedResponse =
            await _cacheHandler.GetCachedResponseAsync<IReadOnlyList<ProductBrand>>(query.RequestDto);

        if (cachedResponse is not null)
            return cachedResponse;

        var productBrands = await _brandRepos.GetEntitiesAsync();

        var timeToLive = TimeSpan.FromSeconds(_cacheConfig.BrandsTimeToLive);
        await _cacheHandler.CacheResponseAsync(query.RequestDto, productBrands, timeToLive);
        
        return productBrands;
    }
}