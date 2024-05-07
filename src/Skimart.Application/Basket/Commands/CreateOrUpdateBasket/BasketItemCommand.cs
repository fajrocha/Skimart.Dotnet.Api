namespace Skimart.Application.Basket.Commands.CreateOrUpdateBasket;

public record BasketItemCommand(
    int Id,
    string ProductName,
    decimal Price,
    int Quantity,
    string PictureUrl,
    string Brand,
    string Type);