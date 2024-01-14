using MediatR;

namespace Skimart.Application.Cases.Basket.Commands.DeleteBasket;

public record DeleteBasketCommand(string Id) : IRequest<bool>;