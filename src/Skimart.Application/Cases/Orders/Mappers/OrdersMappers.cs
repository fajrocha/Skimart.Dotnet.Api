using Skimart.Application.Cases.Orders.Commands.CreateOrder;
using Skimart.Domain.Entities.Order;

namespace Skimart.Application.Cases.Orders.Mappers;

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