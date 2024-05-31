using Skimart.Domain.Entities.Products;

namespace Skimart.Domain.Entities.Order;

public class Order : BaseEntity
{
    public Order()
    {
    }
    
    public Order(List<OrderItem> orderItems, string buyerEmail, ShippingAddress shippingAddress, 
        DeliveryMethod deliveryMethod, string paymentIntentId)
    {
        BuyerEmail = buyerEmail;
        ShippingAddress = shippingAddress;
        DeliveryMethod = deliveryMethod;
        OrderItems = orderItems;
        PaymentIntentId = paymentIntentId;
    }

    public string BuyerEmail { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public ShippingAddress ShippingAddress { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new();
        
    public decimal Subtotal
    {
        get => OrderItems.Sum(item => item.Price * item.Quantity);
        private set { }
    }
    public decimal Total => Subtotal + DeliveryMethod.Price;

    public OrderStatus Status { get; set; }
    public string PaymentIntentId { get; set; }
}