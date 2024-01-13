using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Skimart.Application.Abstractions.Memory.Cache;
using Skimart.Application.Abstractions.Persistence.Repositories.StoreProduct;
using Skimart.Application.Cases.Products.Dtos;
using Skimart.Application.Cases.Shared.Vms;
using Skimart.Application.Configurations.Memory;
using Skimart.Application.Helpers;

namespace Skimart.Application.Cases.Products.Queries.GetAllProducts;

public class GetAllProductHandler : IRequestHandler<GetAllProductsQuery, PaginatedDataVm<ProductToReturnDto>>
{
    private readonly ILogger<GetAllProductHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepos;
    private readonly ICacheHandler _cacheHandler;
    private readonly CacheConfig _cacheConfig;

    public GetAllProductHandler(
        ILogger<GetAllProductHandler> logger,
        IOptions<CacheConfig> opts,
        IMapper mapper, 
        IProductRepository productRepos,
        ICacheHandler cacheHandler)
    {
        _logger = logger;
        _cacheConfig = opts.Value;
        _mapper = mapper;
        _productRepos = productRepos;
        _cacheHandler = cacheHandler;
    }
    
    public async Task<PaginatedDataVm<ProductToReturnDto>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var (productParams, requestDto) = query;

        var cachedResponse =
            await _cacheHandler.GetCachedResponseAsync<PaginatedDataVm<ProductToReturnDto>>(requestDto);

        if (cachedResponse is not null)
            return cachedResponse;
        
        var productCount = await _productRepos.CountAsync(productParams);
        var products = await _productRepos.GetEntitiesAsync(productParams);
        
        var productsDto = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);
        var paginatedProducts = new PaginatedDataVm<ProductToReturnDto>(
            productParams.PageIndex, 
            productParams.PageSize, 
            productCount,
            productsDto);

        var timeToLive = TimeSpan.FromSeconds(_cacheConfig.ProductsTimeToLive);
        await _cacheHandler.CacheResponseAsync(requestDto, paginatedProducts, timeToLive);

        return paginatedProducts;
    }
}