using Application.Abstractions.Persistence.Repositories.StoreProduct;
using Application.Cases.Products.Dtos;
using Application.Cases.Shared.Vms;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Cases.Products.Queries.GetAllProducts;

public class GetAllProductHandler : IRequestHandler<GetAllProductsQuery, PaginatedDataVm<ProductToReturnDto>>
{
    private readonly ILogger<GetAllProductHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepos;

    public GetAllProductHandler(ILogger<GetAllProductHandler> logger, IMapper mapper, IProductRepository productRepos)
    {
        _logger = logger;
        _mapper = mapper;
        _productRepos = productRepos;
    }
    
    public async Task<PaginatedDataVm<ProductToReturnDto>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var (productParams, _) = query;
        
        var productCount = await _productRepos.CountAsync(productParams);
        var products = await _productRepos.GetEntitiesAsync(productParams);
        
        var productsDto = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);
        var paginatedProducts = new PaginatedDataVm<ProductToReturnDto>(
            productParams.PageIndex, 
            productParams.PageSize, 
            productCount,
            productsDto);

        return paginatedProducts;
    }
}