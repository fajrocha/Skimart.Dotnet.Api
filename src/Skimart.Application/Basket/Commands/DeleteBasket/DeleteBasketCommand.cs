using ErrorOr;
using MediatR;

namespace Skimart.Application.Basket.Commands.DeleteBasket;

public record DeleteBasketCommand(string Id) : IRequest<ErrorOr<Deleted>>;