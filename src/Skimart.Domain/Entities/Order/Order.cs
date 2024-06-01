using Skimart.Domain.Entities.Products;

namespace Skimart.Domain.Entities.Order;

public class Order : BaseEntity
{
    public Order()
    {
    }
    
    public Order(string buyerEmail, ShippingAddress shippingAddress, 
        DeliveryMethod deliveryMethod, string paymentIntentId)
    {
        BuyerEmail = buyerEmail;
        ShippingAddress = shippingAddress;
        DeliveryMethod = deliveryMethod;
        PaymentIntentId = paymentIntentId;
    }

    public string BuyerEmail { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public ShippingAddress ShippingAddress { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public List<OrderItem> OrderItems { get; private set; } = new();
        
    public decimal Subtotal
    {
        get => OrderItems.Sum(item => item.Price * item.Quantity);
        private set { }
    }
    public decimal Total => Subtotal + DeliveryMethod.Price;

    public OrderStatus Status { get; set; }
    public string PaymentIntentId { get; set; }

    public void AddOrUpdateOrderItems(List<OrderItem> itemsToAdd)
    {
        var orderItemsToAddHashmap = itemsToAdd.ToDictionary(oi => oi.ItemOrdered.ProductItemId);

        // Remove items that are no longer in basket:
        OrderItems.RemoveAll(orderItem => !orderItemsToAddHashmap.ContainsKey(orderItem.ItemOrdered.ProductItemId));
        
        OrderItems.ForEach(orderItem =>
        {
            var productId = orderItem.ItemOrdered.ProductItemId;
            
            if (orderItemsToAddHashmap.TryGetValue(productId, out var orderItemToAdd))
            {
                _ = orderItemToAdd ?? throw new NullReferenceException($"Could not get value for key {orderItem.Id}.");
                
                orderItem.Quantity = orderItemToAdd.Quantity;
                // Remove from hashmap to then iterate over it to add new products:
                orderItemsToAddHashmap.Remove(orderItem.ItemOrdered.ProductItemId);
            }
        });
        
        foreach (var orderItemToAddKvp in orderItemsToAddHashmap)
        {
            OrderItems.Add(orderItemToAddKvp.Value);
        }
    }
}