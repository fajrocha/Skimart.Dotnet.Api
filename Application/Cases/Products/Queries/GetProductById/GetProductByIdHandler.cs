using Application.Abstractions.Persistence.Repositories.StoreProduct;
using Application.Cases.Products.Dtos;
using Application.Cases.Products.Errors;
using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Cases.Products.Queries.GetProductById;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Result<ProductToReturnDto>>
{
    private readonly ILogger<GetProductByIdHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepos;

    public GetProductByIdHandler(ILogger<GetProductByIdHandler> logger, IMapper mapper, IProductRepository productRepos)
    {
        _logger = logger;
        _mapper = mapper;
        _productRepos = productRepos;
    }
    
    public async Task<Result<ProductToReturnDto>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var (id, _) = query;
        
        var product = await _productRepos.GetEntityByIdAsync(id);

        if (product is null)
            return Result.Fail<ProductToReturnDto>(ProductError.NotFound);
        
        var productDto = _mapper.Map<ProductToReturnDto>(product);

        return Result.Ok(productDto);
    }
}