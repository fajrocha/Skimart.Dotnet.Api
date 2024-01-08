namespace Application.Cases.Products.Dtos;

public record ProductToReturnDto(int Id, string Name, string Description, decimal Price, string PictureUrl,
    string ProductType, string ProductBrand);