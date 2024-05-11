using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Skimart.Application.Configurations.Memory;
using Skimart.Application.Gateways.Memory.Cache;
using Skimart.Application.Gateways.Persistence.Repositories.StoreProduct;
using Skimart.Domain.Entities.Products;

namespace Skimart.Application.Products.Queries.GetAllProductBrands;

public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, List<ProductBrand>>
{
    private readonly ILogger<GetAllBrandsHandler> _logger;
    private readonly IProductBrandRepository _brandRepos;
    private readonly ICacheHandler _cacheHandler;
    private readonly CacheConfiguration _cacheConfiguration;

    public GetAllBrandsHandler(
        ILogger<GetAllBrandsHandler> logger,
        IOptions<CacheConfiguration> opts,
        IProductBrandRepository brandRepos,
        ICacheHandler cacheHandler)
    {
        _logger = logger;
        _cacheConfiguration = opts.Value;
        _brandRepos = brandRepos;
        _cacheHandler = cacheHandler;
    }
    
    public async Task<List<ProductBrand>> Handle(GetAllBrandsQuery query, CancellationToken cancellationToken)
    {
        var productBrands = await _brandRepos.GetEntitiesAsync();

        return productBrands;
    }
}