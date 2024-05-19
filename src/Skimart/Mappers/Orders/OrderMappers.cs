using System.Collections.Generic;
using Skimart.Application.Cases.Orders.Commands.CreateOrder;
using Skimart.Contracts.Orders.Requests;
using Skimart.Contracts.Orders.Responses;
using Skimart.Domain.Entities.Order;

namespace Skimart.Mappers.Orders;

public static class OrderMappers
{
    public static CreateOrderCommand ToCommand(this OrderRequest orderRequest)
    {
        return new CreateOrderCommand(
            orderRequest.BasketId,
            orderRequest.DeliveryMethodId,
            orderRequest.ShippingAddress.ToCommand());
    }
    
    private static CreateOrderShippingAddressCommand ToCommand(this ShippingAddressRequest shippingAddressRequest)
    {
        return new CreateOrderShippingAddressCommand(
            shippingAddressRequest.FirstName,
            shippingAddressRequest.LastName,
            shippingAddressRequest.Street,
            shippingAddressRequest.City,
            shippingAddressRequest.Province,
            shippingAddressRequest.ZipCode);
    }

    public static OrderStatusResponse ToResponse(this OrderStatus orderStatus)
    {
        return orderStatus switch
        {
            OrderStatus.Pending => OrderStatusResponse.Pending,
            OrderStatus.PaymentReceived => OrderStatusResponse.PaymentReceived,
            OrderStatus.PaymentFailed => OrderStatusResponse.PaymentFailed,
            _ => throw new ArgumentOutOfRangeException(nameof(orderStatus)),
        };
    }

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
    
    public static List<OrderResponse> ToResponse(this List<Order> orders)
    {
        return orders.ConvertAll(o => o.ToResponse());
    }

    public static ShippingAddressResponse ToResponse(this ShippingAddress shippingAddress)
    {
        return new ShippingAddressResponse(
            shippingAddress.FirstName,
            shippingAddress.LastName,
            shippingAddress.Street,
            shippingAddress.City,
            shippingAddress.Province,
            shippingAddress.ZipCode);
    }

    public static OrderItemResponse ToResponse(this OrderItem orderItem)
    {
        return new OrderItemResponse(
            orderItem.Id,
            orderItem.ItemOrdered.ToResponse(),
            orderItem.Price,
            orderItem.Quantity);
    }
    
    public static List<OrderItemResponse> ToResponse(this List<OrderItem> source)
    {
        return source.ConvertAll(s => s.ToResponse());
    }
    
    public static List<DeliveryMethodResponse> ToResponse(this List<DeliveryMethod> deliveryMethods)
    {
        return deliveryMethods.ConvertAll(dm => dm.ToResponse());
    }
    
    private static ProductItemOrderedResponse ToResponse(this ProductItemOrdered productItemOrdered)
    {
        return new ProductItemOrderedResponse(
            productItemOrdered.ProductItemId,
            productItemOrdered.ProductName,
            productItemOrdered.PictureUrl);
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