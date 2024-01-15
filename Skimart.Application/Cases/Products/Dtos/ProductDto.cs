namespace Skimart.Application.Cases.Products.Dtos;

public record ProductDto(int Id, string Name, string Description, decimal Price, string PictureUrl,
    string ProductType, string ProductBrand);