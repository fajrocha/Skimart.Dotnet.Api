namespace Skimart.Contracts.Orders.Responses;

public record ProductItemOrderedResponse(
    int ProductItemId,
    string ProductName,
    string PictureUrl);
