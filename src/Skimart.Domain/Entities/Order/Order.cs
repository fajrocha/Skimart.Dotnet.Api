namespace Skimart.Domain.Entities.Order;

public class Order : BaseEntity
{
    public Order()
    {
    }
    
    public Order(IReadOnlyList<OrderItem> orderItems, string buyerEmail, ShippingAddress shippingAddress, 
        DeliveryMethod deliveryMethod, decimal subtotal, string paymentIntentId)
    {
        BuyerEmail = buyerEmail;
        ShippingAddress = shippingAddress;
        DeliveryMethod = deliveryMethod;
        OrderItems = orderItems;
        Subtotal = subtotal;
        PaymentIntentId = paymentIntentId;
    }

    public string BuyerEmail { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public ShippingAddress ShippingAddress { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public IReadOnlyList<OrderItem> OrderItems { get; set; }
    public decimal Subtotal { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string PaymentIntentId { get; set; }
    
    public decimal GetTotal()
    {
        return Subtotal + DeliveryMethod.Price;
    }
}