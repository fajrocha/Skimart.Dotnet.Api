using Skimart.Contracts.Basket.Responses;

namespace Skimart.Contracts.Basket.Requests;

public record CustomerBasketRequest(
    string Id,
    List<BasketItemRequest> Items,
    decimal ShippingPrice,
    int? DeliveryMethodId,
    string ClientSecret = "",
    string PaymentIntentId = "");