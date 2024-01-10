using Application.Abstractions.Persistence.Repositories.StoreProduct;
using Domain.Entities.Product;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Cases.Products.Queries.GetAllProductTypes;

public class GetAllTypesHandler : IRequestHandler<GetAllTypesQuery, IReadOnlyList<ProductType>>
{
    private readonly ILogger<GetAllTypesHandler> _logger;
    private readonly IProductTypeRepository _typesRepos;

    public GetAllTypesHandler(ILogger<GetAllTypesHandler> logger, IProductTypeRepository typesRepos)
    {
        _logger = logger;
        _typesRepos = typesRepos;
    }
    
    public async Task<IReadOnlyList<ProductType>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
    {
        return await _typesRepos.GetEntitiesAsync();
    }
}