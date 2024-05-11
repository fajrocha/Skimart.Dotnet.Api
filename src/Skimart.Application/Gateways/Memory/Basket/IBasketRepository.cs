using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Gateways.Memory.Basket;

public interface IBasketRepository
{
    Task<CustomerBasket?> GetBasketAsync(string basketId);
    Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket);
    Task<bool> DeleteBasketAsync(string basketId);
}