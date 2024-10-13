using ErrorOr;
using MediatR;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Basket.Commands.CreateOrUpdateBasket;

public record CreateOrUpdateBasketCommand(
    string Id,
    List<BasketItemCommand> Items,
    decimal ShippingPrice,
    int? DeliveryMethodId,
    string ClientSecret,
    string PaymentIntentId) : IRequest<ErrorOr<CustomerBasket>>;