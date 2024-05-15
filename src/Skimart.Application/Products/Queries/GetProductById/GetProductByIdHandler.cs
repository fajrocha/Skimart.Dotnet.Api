using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Skimart.Application.Products.Gateways;
using Skimart.Domain.Entities.Products;

namespace Skimart.Application.Products.Queries.GetProductById;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ErrorOr<Product>>
{
    private readonly ILogger<GetProductByIdHandler> _logger;
    private readonly IProductRepository _productRepos;

    public GetProductByIdHandler(
        ILogger<GetProductByIdHandler> logger,
        IProductRepository productRepos)
    {
        _logger = logger;
        _productRepos = productRepos;
    }
    
    public async Task<ErrorOr<Product>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await _productRepos.GetEntityByIdAsync(query.Id);

        if (product is null)
        {
            _logger.LogError("Could not find the product with id {productId}", query.Id);
            return Error.NotFound(description: "Could not find the product for the given id.");
        }
        
        return product;
    }
}