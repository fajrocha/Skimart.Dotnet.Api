using System.Runtime.Serialization;
using Mapster;
using Skimart.Application.Basket.Commands.CreateOrUpdateBasket;
using Skimart.Application.Identity.Commands.Register;
using Skimart.Contracts.Basket.Requests;
using Skimart.Contracts.Basket.Responses;
using Skimart.Contracts.Responses.Basket;
using Skimart.Domain.Entities.Auth;
using Skimart.Domain.Entities.Basket;
using Skimart.Domain.Entities.Order;

namespace Skimart.Mappers;

public class MapsterMappingProfiles : IRegister
{
    private static string ApiUrl => Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(';').FirstOrDefault() 
                                    ?? string.Empty;
    
    public void Register(TypeAdapterConfig config)
    {
        AddBasketMappings(config);
        AddAuthMappings(config);
        AddOrderMappings(config);
    }

    private static void AddBasketMappings(TypeAdapterConfig config)
    {
        config.ForType<BasketItemRequest, BasketItemCommand>().MapToConstructor(true);
        config.ForType<CustomerBasketRequest, CreateOrUpdateBasketCommand>().MapToConstructor(true);
        
        config.ForType<BasketItem, BasketItemResponse>().MapToConstructor(true);
        config.ForType<CustomerBasket, CustomerBasketResponse>().MapToConstructor(true);
    }
    
    private static void AddAuthMappings(TypeAdapterConfig config)
    {
        config.ForType<RegisterCommand, AppUser>().Map(d => d.UserName, s => s.Email);
    }
    
    private static void AddOrderMappings(TypeAdapterConfig config)
    {
    }
    
    private static string GetEnumMemberValue(OrderStatus status)
    {
        var enumMemberAttribute = typeof(OrderStatus)?.GetField(status.ToString())?
            .GetCustomAttributes(typeof(EnumMemberAttribute), true)
            .FirstOrDefault() as EnumMemberAttribute;

        return enumMemberAttribute?.Value ?? status.ToString();
    }
}