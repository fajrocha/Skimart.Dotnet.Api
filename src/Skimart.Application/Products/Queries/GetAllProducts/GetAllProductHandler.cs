using MediatR;
using Microsoft.Extensions.Logging;
using Skimart.Application.Products.Gateways;

namespace Skimart.Application.Products.Queries.GetAllProducts;

public class GetAllProductHandler : IRequestHandler<GetAllProductsQuery, ProductsResponseDto>
{
    private readonly ILogger<GetAllProductHandler> _logger;
    private readonly IProductRepository _productRepos;

    public GetAllProductHandler(
        ILogger<GetAllProductHandler> logger,
        IProductRepository productRepos)
    {
        _logger = logger;
        _productRepos = productRepos;
    }
    
    public async Task<ProductsResponseDto> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var productCount = await _productRepos.CountAsync(query);
        _logger.LogDebug("Retrieved the following amount of products: {productCount}", productCount);
        var products = await _productRepos.GetEntitiesAsync(query);
        
        return new ProductsResponseDto(productCount, products);
    }
}