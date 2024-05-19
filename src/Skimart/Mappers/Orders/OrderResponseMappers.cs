using Skimart.Contracts.Orders.Responses;
using Skimart.Domain.Entities.Order;

namespace Skimart.Mappers.Orders;

public static class OrderResponseMappers
{
    public static OrderResponse ToResponse(this Order order)
    {
        return new OrderResponse(
            order.Id,
            order.BuyerEmail,
            order.OrderDate,
            order.ShippingAddress.ToResponse(),
            order.DeliveryMethod.ToResponse(),
            order.OrderItems.ToResponse(),
            order.Subtotal,
            order.Status.ToResponse(),
            order.PaymentIntentId);
    }
    
    public static List<DeliveryMethodResponse> ToResponse(this List<DeliveryMethod> deliveryMethods)
    {
        return deliveryMethods.ConvertAll(dm => dm.ToResponse());
    }
    
    public static List<OrderResponse> ToResponse(this List<Order> orders)
    {
        return orders.ConvertAll(o => o.ToResponse());
    }
    
    private static List<OrderItemResponse> ToResponse(this List<OrderItem> source)
    {
        return source.ConvertAll(s => s.ToResponse());
    }

    private static OrderItemResponse ToResponse(this OrderItem orderItem)
    {
        return new OrderItemResponse(
            orderItem.Id,
            orderItem.ItemOrdered.ToResponse(),
            orderItem.Price,
            orderItem.Quantity);
    }
    
    private static ProductItemOrderedResponse ToResponse(this ProductItemOrdered productItemOrdered)
    {
        return new ProductItemOrderedResponse(
            productItemOrdered.ProductItemId,
            productItemOrdered.ProductName,
            productItemOrdered.PictureUrl);
    }
    
    private static ShippingAddressResponse ToResponse(this ShippingAddress shippingAddress)
    {
        return new ShippingAddressResponse(
            shippingAddress.FirstName,
            shippingAddress.LastName,
            shippingAddress.Street,
            shippingAddress.City,
            shippingAddress.Province,
            shippingAddress.ZipCode);
    }
    
    private static OrderStatusResponse ToResponse(this OrderStatus orderStatus)
    {
        return orderStatus switch
        {
            OrderStatus.Pending => OrderStatusResponse.Pending,
            OrderStatus.PaymentReceived => OrderStatusResponse.PaymentReceived,
            OrderStatus.PaymentFailed => OrderStatusResponse.PaymentFailed,
            _ => throw new ArgumentOutOfRangeException(nameof(orderStatus)),
        };
    }
    
    private static DeliveryMethodResponse ToResponse(this DeliveryMethod deliveryMethod)
    {
        return new DeliveryMethodResponse(
            deliveryMethod.Id,
            deliveryMethod.ShortName,
            deliveryMethod.DeliveryTime,
            deliveryMethod.Description,
            deliveryMethod.Price);
    }
}