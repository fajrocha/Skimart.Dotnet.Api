using System.ComponentModel.DataAnnotations;

namespace Skimart.Contracts.Basket.Requests;

public record BasketItemRequest(int Id,
    string ProductName,
    decimal Price,
    int Quantity,
    string PictureUrl,
    string Brand,
    string Type);