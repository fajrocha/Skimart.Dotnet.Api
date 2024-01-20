using FluentResults;
using MediatR;
using Skimart.Application.Cases.Basket.Dtos;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Cases.Basket.Commands.CreateOrUpdateBasket;

public record CreateOrUpdateBasketCommand(CustomerBasketDto CustomerBasketDto) : IRequest<Result<CustomerBasket>>;