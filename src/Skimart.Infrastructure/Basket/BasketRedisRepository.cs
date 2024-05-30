using System.Text.Json;
using Microsoft.Extensions.Options;
using Skimart.Application.Basket.Configurations;
using Skimart.Application.Basket.Gateways;
using Skimart.Domain.Entities.Basket;
using StackExchange.Redis;

namespace Skimart.Infrastructure.Basket;

public class BasketRedisRepository : IBasketRepository
{
    private readonly IDatabase _redis;
    private readonly BasketConfiguration _basketConfiguration;

    public BasketRedisRepository(IOptions<BasketConfiguration> basketConfig, IConnectionMultiplexer redis)
    {
        _basketConfiguration = basketConfig.Value;
        _redis = redis.GetDatabase();
    }
    
    public async Task<CustomerBasket?> GetBasketAsync(string basketId)
    {
        var customerBasket = await _redis.StringGetAsync(basketId);
        
        return customerBasket.IsNullOrEmpty ? 
            null : 
            JsonSerializer.Deserialize<CustomerBasket>(customerBasket!);
    }

    public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket)
    {
        var timeToLive = TimeSpan.FromDays(_basketConfiguration.TimeToLive);
        var success = await _redis.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), timeToLive);

        return success ? await GetBasketAsync(basket.Id) : null;
    }

    public async Task<bool> DeleteBasketAsync(string basketId)
    {
        return await _redis.KeyDeleteAsync(basketId);
    }
}