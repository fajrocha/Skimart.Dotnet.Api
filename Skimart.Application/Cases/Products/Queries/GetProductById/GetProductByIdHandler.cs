﻿using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Skimart.Application.Abstractions.Memory.Cache;
using Skimart.Application.Abstractions.Persistence.Repositories.StoreProduct;
using Skimart.Application.Cases.Products.Dtos;
using Skimart.Application.Cases.Products.Errors;
using Skimart.Application.Configurations.Memory;

namespace Skimart.Application.Cases.Products.Queries.GetProductById;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Result<ProductToReturnDto>>
{
    private readonly ILogger<GetProductByIdHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepos;
    private readonly ICacheHandler _cacheHandler;
    private readonly CacheConfig _cacheConfig;

    public GetProductByIdHandler(
        ILogger<GetProductByIdHandler> logger,
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
    
    public async Task<Result<ProductToReturnDto>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var (id, requestDto) = query;
        var cachedResponse = await _cacheHandler.GetCachedResponseAsync<ProductToReturnDto>(requestDto);

        if (cachedResponse is not null)
            return Result.Ok(cachedResponse);
        
        var product = await _productRepos.GetEntityByIdAsync(id);

        if (product is null)
            return Result.Fail<ProductToReturnDto>(ProductError.NotFound);
        
        var productDto = _mapper.Map<ProductToReturnDto>(product);

        var timeToLive = TimeSpan.FromSeconds(_cacheConfig.ProductsTimeToLive);
        await _cacheHandler.CacheResponseAsync(productDto, timeToLive);

        return Result.Ok(productDto);
    }
}