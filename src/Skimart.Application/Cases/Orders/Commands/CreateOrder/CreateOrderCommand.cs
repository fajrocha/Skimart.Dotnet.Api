using ErrorOr;
using MediatR;
using Skimart.Application.Cases.Orders.Dtos;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Cases.Orders.Commands.CreateOrder;

public record CreateOrderCommand(string BasketId, int DeliveryMethodId, CreateOrderShippingAddressCommand ShippingAddress) 
    : IRequest<ErrorOr<Order>>;