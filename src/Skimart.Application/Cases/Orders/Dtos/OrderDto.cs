using Skimart.Application.Identity.DTOs;

namespace Skimart.Application.Cases.Orders.Dtos;

public record OrderDto(string BasketId, int DeliveryMethodId, AddressDto ShippingAddress);