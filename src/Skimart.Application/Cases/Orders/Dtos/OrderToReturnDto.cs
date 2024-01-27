using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Cases.Orders.Dtos;

public record OrderToReturnDto(
    int Id, 
    string BuyerEmail, 
    DateTime OrderDate, 
    ShippingAddress ShippingAddress, 
    string DeliveryMethod, 
    decimal ShippingPrice, 
    IReadOnlyList<OrderItemDto> OrderItems, 
    decimal Subtotal, 
    decimal Total, 
    string Status);