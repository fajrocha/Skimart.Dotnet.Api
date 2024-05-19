namespace Skimart.Contracts.Orders.Responses;

public record OrderItemResponse(
    int Id,
    ProductItemOrderedResponse ItemOrdered,
    decimal Price,
    int Quantity);