using ErrorOr;
using MediatR;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Basket.Queries.GetBasketById;

public record GetBasketByIdQuery(string Id) : IRequest<ErrorOr<CustomerBasket>>;