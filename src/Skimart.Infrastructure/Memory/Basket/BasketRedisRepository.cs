using System.Text.Json;
using Microsoft.Extensions.Options;
using Skimart.Application.Abstractions.Memory.Basket;
using Skimart.Application.Configurations.Memory;
using Skimart.Domain.Entities.Basket;
using StackExchange.Redis;

namespace Skimart.Infrastructure.Memory.Basket;

public class BasketRedisRepository : IBasketRepository
{
    private readonly IDatabase _redis;
    private readonly BasketConfig _basketConfig;

    public BasketRedisRepository(IOptions<BasketConfig> basketConfig,IConnectionMultiplexer redis)
    {
        _basketConfig = basketConfig.Value;
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
        var timeToLive = TimeSpan.FromDays(_basketConfig.TimeToLive);
        var success = await _redis.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), timeToLive);

        return success ? await GetBasketAsync(basket.Id) : null;
    }

    public async Task<bool> DeleteBasketAsync(string basketId)
    {
        return await _redis.KeyDeleteAsync(basketId);
    }
}