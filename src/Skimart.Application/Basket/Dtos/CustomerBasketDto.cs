using System.ComponentModel.DataAnnotations;
using Skimart.Application.Cases.Basket.Dtos;

namespace Skimart.Application.Basket.Dtos;

public record CustomerBasketDto(
    string Id,
    List<BasketItemDto> Items,
    decimal ShippingPrice,
    int? DeliveryMethodId,
    string ClientSecret = "",
    string PaymentIntentId = "");