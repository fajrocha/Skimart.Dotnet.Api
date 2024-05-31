using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Skimart.Application.Basket.Errors;
using Skimart.Application.Basket.Gateways;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Basket.Queries.GetBasketById;

public class GetBasketByIdHandler : IRequestHandler<GetBasketByIdQuery, ErrorOr<CustomerBasket>>
{
    private readonly ILogger<GetBasketByIdHandler> _logger;
    private readonly IBasketRepository _basketRepos;

    public GetBasketByIdHandler(ILogger<GetBasketByIdHandler> logger, IBasketRepository basketRepos)
    {
        _logger = logger;
        _basketRepos = basketRepos;
    }
    
    public async Task<ErrorOr<CustomerBasket>> Handle(GetBasketByIdQuery query, CancellationToken cancellationToken)
    {
        var customerBasket = await _basketRepos.GetBasketAsync(query.Id);

        if (customerBasket is null)
        {
            _logger.LogWarning("Basket with id {basketId} not found.", query.Id);
            return Error.NotFound(description: BasketErrors.BasketNotFound);
        }

        return customerBasket;
    }
}