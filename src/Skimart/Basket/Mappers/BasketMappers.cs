using Skimart.Application.Basket.Commands.CreateOrUpdateBasket;
using Skimart.Contracts.Basket.Requests;
using Skimart.Contracts.Basket.Responses;
using Skimart.Contracts.Responses.Basket;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Basket.Mappers;

public static class BasketMappers
{
    public static CustomerBasketResponse ToResponse(this CustomerBasket customerBasket)
    {
        return new CustomerBasketResponse(
            customerBasket.Id,
            customerBasket.Items.ConvertAll(basketItem => basketItem.ToResponse()),
            customerBasket.ShippingPrice,
            customerBasket.DeliveryMethodId,
            customerBasket.ClientSecret,
            customerBasket.PaymentIntentId);
    }

    public static CreateOrUpdateBasketCommand ToCommand(this CustomerBasketRequest customerBasketRequest)
    {
        return new CreateOrUpdateBasketCommand(
            customerBasketRequest.Id,
            customerBasketRequest.Items.ConvertAll(basketItem => basketItem.ToCommand()),
            customerBasketRequest.ShippingPrice,
            customerBasketRequest.DeliveryMethodId,
            customerBasketRequest.ClientSecret,
            customerBasketRequest.PaymentIntentId);
    }

    private static BasketItemResponse ToResponse(this BasketItem basketItem)
    {
        return new BasketItemResponse(
            basketItem.Id,
            basketItem.ProductName,
            basketItem.Price,
            basketItem.Quantity,
            basketItem.PictureUrl,
            basketItem.Brand,
            basketItem.Type);
    }
    
    private static BasketItemCommand ToCommand(this BasketItemRequest basketItemCommand)
    {
        return new BasketItemCommand(
            basketItemCommand.Id,
            basketItemCommand.ProductName,
            basketItemCommand.Price,
            basketItemCommand.Quantity,
            basketItemCommand.PictureUrl,
            basketItemCommand.Brand,
            basketItemCommand.Type);
    }
}