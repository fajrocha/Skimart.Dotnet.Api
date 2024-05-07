using Skimart.Application.Basket.Commands.CreateOrUpdateBasket;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Basket.Mappers;

public static class BasketMappers
{
    public static CustomerBasket ToDomain(this CreateOrUpdateBasketCommand customerBasket)
    {
        return new CustomerBasket(
            customerBasket.Id,
            customerBasket.Items.ConvertAll(basketItem => basketItem.ToDomain()),
            customerBasket.ShippingPrice,
            customerBasket.DeliveryMethodId,
            customerBasket.ClientSecret,
            customerBasket.PaymentIntentId);
    }
    
    private static BasketItem ToDomain(this BasketItemCommand basketItemCommand)
    {
        return new BasketItem(
            basketItemCommand.Id,
            basketItemCommand.ProductName,
            basketItemCommand.Price,
            basketItemCommand.Quantity,
            basketItemCommand.PictureUrl,
            basketItemCommand.Brand,
            basketItemCommand.Type);
    }
}