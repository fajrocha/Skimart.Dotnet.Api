using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Skimart.Application.Abstractions.Memory.Basket;
using Skimart.Application.Cases.Basket.Errors;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Cases.Basket.Queries.GetBasketById;

public class GetBasketByIdHandler : IRequestHandler<GetBasketByIdQuery, Result<CustomerBasket>>
{
    private readonly ILogger<GetBasketByIdHandler> _logger;
    private readonly IBasketRepository _basketRepos;

    public GetBasketByIdHandler(ILogger<GetBasketByIdHandler> logger, IBasketRepository basketRepos)
    {
        _logger = logger;
        _basketRepos = basketRepos;
    }
    
    public async Task<Result<CustomerBasket>> Handle(GetBasketByIdQuery query, CancellationToken cancellationToken)
    {
        var customerBasket = await _basketRepos.GetBasketAsync(query.Id);

        return customerBasket is not null ? Result.Ok(customerBasket) : Result.Fail(CustomerBasketError.NotFound);
    }
}