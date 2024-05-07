using Skimart.Contracts.Basket.Responses;

namespace Skimart.Contracts.Responses.Basket;

public record CustomerBasketResponse(
    string Id,
    List<BasketItemResponse> Items,
    decimal ShippingPrice,
    int? DeliveryMethodId,
    string ClientSecret,
    string PaymentIntentId);