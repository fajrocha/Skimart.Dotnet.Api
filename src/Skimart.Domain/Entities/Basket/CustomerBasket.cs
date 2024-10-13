namespace Skimart.Domain.Entities.Basket;

public class CustomerBasket
{
    public string Id { get; init; }
    public List<BasketItem> Items { get; init; }
    public decimal ShippingPrice { get; init; }
    public int? DeliveryMethodId { get; init; }
    public string ClientSecret { get; set; }
    public string PaymentIntentId { get; set; }
    
    public CustomerBasket(
        string id,
        List<BasketItem> items,
        decimal shippingPrice,
        int? deliveryMethodId,
        string clientSecret = "",
        string paymentIntentId = "")
    {
        Id = id;
        Items = items;
        ShippingPrice = shippingPrice;
        DeliveryMethodId = deliveryMethodId;
        ClientSecret = clientSecret;
        PaymentIntentId = paymentIntentId;
    }
}