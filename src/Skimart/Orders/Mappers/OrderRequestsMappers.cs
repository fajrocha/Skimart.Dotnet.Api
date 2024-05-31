using Skimart.Application.Orders.Commands.CreateOrder;
using Skimart.Contracts.Orders.Requests;

namespace Skimart.Orders.Mappers;

public static class OrderRequestsMappers
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
}