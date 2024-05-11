using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using Skimart.Application.Cases.Shared.Dtos;
using Skimart.Application.Gateways.Memory.Cache;
using Skimart.Application.Helpers;

namespace Skimart.Application.UnitTests.Helpers;

public class CacheHandlerTests
{
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Fixture _fixture;

    public CacheHandlerTests()
    {
        _cacheServiceMock = new Mock<ICacheService>();
        _fixture = new Fixture();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task GetCachedResponseAsync_WhenNoCache_ReturnsNullObject(string? cachedDataAsString)
    {
        var queryCollection = new QueryCollection();
        var requestDto = new HttpRequestDto("/api/", queryCollection);
        
        _cacheServiceMock.Setup(cs => cs.GetCachedResponseAsync(It.IsAny<string>())).ReturnsAsync(cachedDataAsString);

        var cacheHandler = new CacheHandler(_cacheServiceMock.Object);

        var cachedData = await cacheHandler.GetCachedResponseAsync<ProductResponse>(requestDto);
        
        Assert.Null(cachedData);
    }
    
    [Fact]
    public async Task GetCachedResponseAsync_WhenThereIsCachedData_ReturnsCachedData()
    {
        var queryCollection = new QueryCollection();
        var requestDto = new HttpRequestDto("/api/", queryCollection);

        var product = _fixture.Create<ProductResponse>();
        var productAsString = SystemJsonSerializer.SerializeCamelCase(product);
        
        _cacheServiceMock.Setup(cs => cs.GetCachedResponseAsync(It.IsAny<string>())).ReturnsAsync(productAsString);

        var cacheHandler = new CacheHandler(_cacheServiceMock.Object);

        var cachedData = await cacheHandler.GetCachedResponseAsync<ProductResponse>(requestDto);
        
        cachedData.ShouldDeepEqual(product);
    }
    
    [Fact]
    public async Task CacheResponseAsync_WhenThereIsCachedData_CacheServiceIsCalled()
    {
        var dict = new Dictionary<string, StringValues>
        {
            { "queryParam1", "1" },
            { "queryParam2", "2" },
        };
        var queryCollection = new QueryCollection(dict);

        var requestDto = new HttpRequestDto("/api/", queryCollection);

        var product = _fixture.Create<ProductResponse>();
        
        var cacheHandler = new CacheHandler(_cacheServiceMock.Object);

        var timeToLive = TimeSpan.FromSeconds(1);
        await cacheHandler.CacheResponseAsync(requestDto, product, timeToLive);
        
        _cacheServiceMock.Verify(cs => cs.CacheResponseAsync(It.IsAny<string>(), product, timeToLive), Times.Once);
    }
}