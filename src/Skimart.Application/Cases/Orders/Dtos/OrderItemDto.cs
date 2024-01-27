namespace Skimart.Application.Cases.Orders.Dtos;

public record OrderItemDto(int ProductId, string ProductName, string PictureUrl, decimal Price, int Quantity);