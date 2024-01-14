using System.ComponentModel.DataAnnotations;

namespace Skimart.Application.Cases.Basket.Dtos;

public record CustomerBasketDto(
    [Required]
    string Id,
    List<BasketItemDto> Items,
    decimal ShippingPrice,
    int? DeliveryMethodId,
    string ClientSecret = "",
    string PaymentIntentId = "");