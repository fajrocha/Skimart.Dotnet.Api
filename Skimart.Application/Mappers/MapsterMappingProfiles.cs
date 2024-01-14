using Domain.Entities.Product;
using Mapster;
using MapsterMapper;
using Skimart.Application.Cases.Basket.Dtos;
using Skimart.Application.Cases.Products.Dtos;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Mappers;

public class MapsterMappingProfiles : IRegister
{
    private static string ApiUrl => Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(';').FirstOrDefault() 
                                    ?? string.Empty;
    
    public void Register(TypeAdapterConfig config)
    {
        AddProductMappings(config);
        AddBasketMappings(config);
    }

    private static void AddProductMappings(TypeAdapterConfig config)
    {
        config.ForType<Product, ProductToReturnDto>()
            .Map(d => d.ProductBrand, s => s.ProductBrand.Name)
            .Map(d => d.ProductType, s => s.ProductType.Name)
            .Map(d => d.PictureUrl, s => $"{ApiUrl}/{s.PictureUrl}")
            .MapToConstructor(true);
    }
    
    private static void AddBasketMappings(TypeAdapterConfig config)
    {
        config.ForType<CustomerBasketDto, CustomerBasket>();
        config.ForType<BasketItemDto, BasketItemDto>();
    }
}