namespace Skimart.Contracts.Orders.Responses;

public record OrderResponse(
    int Id,
    string BuyerEmail,
    DateTime OrderDate,
    ShippingAddressResponse ShippingAddress,
    DeliveryMethodResponse DeliveryMethod,
    List<OrderItemResponse> OrderItems,
    decimal Subtotal,
    OrderStatusResponse Status,
    string PaymentIntentId);