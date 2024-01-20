using FluentResults;
using MediatR;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Cases.Basket.Queries.GetBasketById;

public record GetBasketByIdQuery(string Id) : IRequest<Result<CustomerBasket>>;