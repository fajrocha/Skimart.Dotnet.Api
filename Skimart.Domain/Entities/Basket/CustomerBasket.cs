namespace Skimart.Domain.Entities.Basket;

public class CustomerBasket
{
    public string Id { get; set; } = string.Empty;
    public List<BasketItem> Items { get; set; } = new();
    public decimal ShippingPrice { get; set; }
    public int? DeliveryMethodId { get; set; }
    public string ClientSecret { get; set; } = string.Empty;
    public string PaymentIntentId { get; set; } = string.Empty;
    
    public CustomerBasket(string id)
    {
        Id = id;
    }

    public CustomerBasket()
    {
    }
}