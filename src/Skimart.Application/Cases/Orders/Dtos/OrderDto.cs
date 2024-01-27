using Skimart.Application.Cases.Auth.Dtos;

namespace Skimart.Application.Cases.Orders.Dtos;

public record OrderDto(string BasketId, int DeliveryMethodId, AddressDto ShippingAddress);