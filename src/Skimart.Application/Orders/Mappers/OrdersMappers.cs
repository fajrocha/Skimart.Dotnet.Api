using Skimart.Application.Orders.Commands.CreateOrder;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Orders.Mappers;

public static class OrdersMappers
{
    public static ShippingAddress ToAddress(this CreateOrderShippingAddressCommand shippingAddress)
    {
        return new ShippingAddress(
            shippingAddress.FirstName,
            shippingAddress.LastName,
            shippingAddress.Street,
            shippingAddress.City,
            shippingAddress.Province,
            shippingAddress.ZipCode);
    }
}