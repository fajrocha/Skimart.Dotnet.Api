using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Abstractions.Memory.Basket;

public interface IBasketRepository
{
    Task<CustomerBasket?> GetBasketAsync(string basketId);
    Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);
    Task<bool> DeleteBasketAsync(string basketId);
}