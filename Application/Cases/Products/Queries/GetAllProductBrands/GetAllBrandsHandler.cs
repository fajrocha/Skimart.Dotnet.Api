using Application.Abstractions.Persistence.Repositories.StoreProduct;
using Domain.Entities.Product;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Cases.Products.Queries.GetAllProductBrands;

public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, IReadOnlyList<ProductBrand>>
{
    private readonly IProductBrandRepository _brandRepos;

    public GetAllBrandsHandler(ILogger<GetAllBrandsHandler> logger, IProductBrandRepository brandRepos)
    {
        _brandRepos = brandRepos;
    }
    
    public async Task<IReadOnlyList<ProductBrand>> Handle(GetAllBrandsQuery query, CancellationToken cancellationToken)
    {
        return await _brandRepos.GetEntitiesAsync();
    }
}