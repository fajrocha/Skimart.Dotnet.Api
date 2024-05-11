using MediatR;
using Microsoft.Extensions.Logging;
using Skimart.Application.Gateways.Persistence.Repositories.StoreProduct;
using Skimart.Domain.Entities.Products;

namespace Skimart.Application.Products.Queries.GetAllProductTypes;

public class GetAllTypesHandler : IRequestHandler<GetAllTypesQuery, List<ProductType>>
{
    private readonly ILogger<GetAllTypesHandler> _logger;
    private readonly IProductTypeRepository _typesRepos;

    public GetAllTypesHandler(
        ILogger<GetAllTypesHandler> logger, 
        IProductTypeRepository typesRepos)
    {
        _logger = logger;
        _typesRepos = typesRepos;
    }
    
    public async Task<List<ProductType>> Handle(GetAllTypesQuery query, CancellationToken cancellationToken)
    {
        var productTypes = await _typesRepos.GetEntitiesAsync();
        
        return productTypes;
    }
}