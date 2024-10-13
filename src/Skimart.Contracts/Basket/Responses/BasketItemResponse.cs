namespace Skimart.Contracts.Basket.Responses;

public record BasketItemResponse(
    int Id,
    string ProductName,
    decimal Price,
    int Quantity,
    string PictureUrl,
    string Brand,
    string Type);