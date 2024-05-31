namespace Skimart.Contracts.Orders.Responses;

public record OrderResponse(
    int Id,
    string BuyerEmail,
    DateTime OrderDate,
    ShippingAddressResponse ShippingAddress,
    string DeliveryMethod,
    List<OrderItemResponse> OrderItems,
    decimal ShippingPrice,
    decimal Subtotal,
    decimal Total,
    OrderStatusResponse Status,
    string PaymentIntentId);