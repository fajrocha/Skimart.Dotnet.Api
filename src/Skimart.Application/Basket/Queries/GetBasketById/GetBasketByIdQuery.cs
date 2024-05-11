using ErrorOr;
using MediatR;
using Skimart.Application.Cache;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Basket.Queries.GetBasketById;

public record GetBasketByIdQuery(string Id) : IRequest<ErrorOr<CustomerBasket>>
{
    public string CacheKey => $"{typeof(GetBasketByIdQuery)}-Request-{DateTime.UtcNow}";
}