using Application.Cases.Products.Dtos;
using Domain.Entities.Product;
using Mapster;

namespace Application.Mappers;

public class MapsterMappingProfiles : IRegister
{
    private static string ApiUrl => Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(';').FirstOrDefault() 
                                    ?? string.Empty;
    
    public void Register(TypeAdapterConfig config)
    {
        AddProductMappings(config);
    }

    private static void AddProductMappings(TypeAdapterConfig config)
    {
        config.ForType<Product, ProductToReturnDto>()
            .Map(d => d.ProductBrand, s => s.ProductBrand.Name)
            .Map(d => d.ProductType, s => s.ProductType.Name)
            .Map(d => d.PictureUrl, s => $"{ApiUrl}/{s.PictureUrl}")
            .MapToConstructor(true);
    }
}