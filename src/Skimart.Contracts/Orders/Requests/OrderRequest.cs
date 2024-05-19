namespace Skimart.Contracts.Orders.Requests;

public record OrderRequest(
    string BasketId, 
    int DeliveryMethodId, 
    ShippingAddressRequest ShippingAddress);