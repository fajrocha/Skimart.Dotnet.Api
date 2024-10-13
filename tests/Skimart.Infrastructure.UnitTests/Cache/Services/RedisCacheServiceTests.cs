using Microsoft.Extensions.Caching.Distributed;
using Skimart.Application.Shared.Extensions;
using Skimart.Infrastructure.Cache.Services;
using Product = Skimart.Domain.Entities.Products.Product;

namespace Skimart.Infrastructure.UnitTests.Cache.Services;

public class RedisCacheServiceTests
{
    private readonly IDistributedCache _mockDistributedCache;
    private readonly RedisCacheService _sut;
    private readonly Fixture _fixture;

    public RedisCacheServiceTests()
    {
        _fixture = new Fixture();
        _mockDistributedCache = Substitute.For<IDistributedCache>();

        _sut = new RedisCacheService(_mockDistributedCache);
    }

    [Fact]
    public async Task SetCacheValueAsync_WhenValueIsNull_ShouldNotStoreIt()
    {
        var key = _fixture.Create<string>();
        var timeToLive = _fixture.Create<TimeSpan>();
        object? value = null;

        await _sut.SetCacheValueAsync(key, value, timeToLive);

        await _mockDistributedCache.DidNotReceive()
            .SetStringAsync(key, value.SerializeCamelCase(), Arg.Any<DistributedCacheEntryOptions>());
    }
}