using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Skimart.Application.Cache.Gateways;
using Skimart.Application.Configurations.Memory;
using Skimart.Application.Products.Gateways;
using Skimart.Domain.Entities.Products;

namespace Skimart.Application.Products.Queries.GetAllProductBrands;

public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, List<ProductBrand>>
{
    private readonly ILogger<GetAllBrandsHandler> _logger;
    private readonly IProductBrandRepository _brandRepos;

    public GetAllBrandsHandler(
        ILogger<GetAllBrandsHandler> logger,
        IProductBrandRepository brandRepos)
    {
        _logger = logger;
        _brandRepos = brandRepos;
    }
    
    public async Task<List<ProductBrand>> Handle(GetAllBrandsQuery query, CancellationToken cancellationToken)
    {
        var productBrands = await _brandRepos.GetEntitiesAsync();

        return productBrands;
    }
}