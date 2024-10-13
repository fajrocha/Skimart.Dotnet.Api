using ErrorOr;
using MediatR;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(string BasketId, int DeliveryMethodId, CreateOrderShippingAddressCommand ShippingAddress) 
    : IRequest<ErrorOr<Order>>;