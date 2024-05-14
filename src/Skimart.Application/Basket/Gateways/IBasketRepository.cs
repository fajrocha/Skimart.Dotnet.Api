using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Basket.Gateways;

public interface IBasketRepository
{
    Task<CustomerBasket?> GetBasketAsync(string basketId);
    Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket);
    Task<bool> DeleteBasketAsync(string basketId);
}