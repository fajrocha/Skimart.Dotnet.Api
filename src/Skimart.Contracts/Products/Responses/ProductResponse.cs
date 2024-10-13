namespace Skimart.Contracts.Products.Responses;

public record ProductResponse(
    int Id, 
    string Name, 
    string Description, 
    decimal Price, 
    string PictureUrl,
    string ProductType, 
    string ProductBrand);