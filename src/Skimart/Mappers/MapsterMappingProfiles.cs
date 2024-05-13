using System.Runtime.Serialization;
using Mapster;
using Skimart.Application.Basket.Commands.CreateOrUpdateBasket;
using Skimart.Application.Cases.Auth.Dtos;
using Skimart.Application.Cases.Orders.Dtos;
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
        config.ForType<Address, AddressDto>().TwoWays().MapToConstructor(true);
    }
    
    private static void AddOrderMappings(TypeAdapterConfig config)
    {
        config.ForType<AddressDto, ShippingAddress>().MapToConstructor(true);
        config.ForType<Order, OrderToReturnDto>()
            .Map(d => d.DeliveryMethod, s => s.DeliveryMethod.ShortName)
            .Map(d => d.ShippingPrice, s => s.DeliveryMethod.Price)
            .Map(d => d.Status, s => GetEnumMemberValue(s.Status))
            .MapToConstructor(true);
        config.ForType<OrderItem, OrderItemDto>()
            .Map(d => d.ProductId, s => s.ItemOrdered.ProductItemId)
            .Map(d => d.ProductName, s => s.ItemOrdered.ProductName)
            .Map(d => d.PictureUrl, s => $"{ApiUrl}/{s.ItemOrdered.PictureUrl}")
            .MapToConstructor(true);
    }
    
    private static string GetEnumMemberValue(OrderStatus status)
    {
        var enumMemberAttribute = typeof(OrderStatus)?.GetField(status.ToString())?
            .GetCustomAttributes(typeof(EnumMemberAttribute), true)
            .FirstOrDefault() as EnumMemberAttribute;

        return enumMemberAttribute?.Value ?? status.ToString();
    }
}