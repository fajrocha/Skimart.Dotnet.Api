namespace Skimart.Contracts.Orders.Responses;

public record DeliveryMethodResponse(
    int Id, 
    string ShortName, 
    string DeliveryTime, 
    string Description, 
    decimal Price);