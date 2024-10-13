namespace Skimart.Contracts.Orders.Responses;

public record OrderItemResponse(
    int Id,
    int ProductItemId,
    string ProductName,
    string PictureUrl,
    decimal Price,
    int Quantity);