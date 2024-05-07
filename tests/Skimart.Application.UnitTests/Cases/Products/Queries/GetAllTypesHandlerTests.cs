using AutoFixture;
using Domain.Entities.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Skimart.Application.Abstractions.Memory.Cache;
using Skimart.Application.Abstractions.Persistence.Repositories.StoreProduct;
using Skimart.Application.Cases.Products.Queries.GetAllProductTypes;
using Skimart.Application.Cases.Shared.Dtos;
using Skimart.Application.Configurations.Memory;

namespace Skimart.Application.UnitTests.Cases.Products.Queries;

public class GetAllTypesHandlerTests
{
    private const int TypesTimeToLive = 1;
    
    private readonly Mock<ILogger<GetAllTypesHandler>> _loggerMock;
    private readonly Mock<IOptions<CacheConfig>> _cacheConfigMock;
    private readonly Mock<IProductTypeRepository> _typesRepositoryMock;
    private readonly Mock<ICacheHandler> _cacheHandlerMock;
    private readonly Fixture _fixture;
    
    public GetAllTypesHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetAllTypesHandler>>();
        _cacheConfigMock = new Mock<IOptions<CacheConfig>>();
        
        _cacheConfigMock.Setup(s => s.Value).Returns(new CacheConfig
        {
            TypesTimeToLive = TypesTimeToLive,
        });
        
        _typesRepositoryMock = new Mock<IProductTypeRepository>();
        _cacheHandlerMock = new Mock<ICacheHandler>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_WhenNoCachedData_ReturnsTypesFromPersistenceAndCachesIt()
    {
        const int typesCount = 4;
        var queryCollection = new QueryCollection();
        var requestDto = new HttpRequestDto("/api/", queryCollection);
        
        var query = new GetAllTypesQuery(requestDto);
        
        IReadOnlyList<ProductType>? cachedResponse = null;
        _cacheHandlerMock.Setup(ch => ch.GetCachedResponseAsync<IReadOnlyList<ProductType>>(requestDto))
            .ReturnsAsync(cachedResponse);
        
        var typesStored = _fixture.CreateMany<ProductType>(typesCount).ToList();
        _typesRepositoryMock.Setup(tr => tr.GetEntitiesAsync()).ReturnsAsync(typesStored);
        
        var handler = new GetAllTypesHandler(
            _loggerMock.Object,
            _cacheConfigMock.Object,
            _typesRepositoryMock.Object,
            _cacheHandlerMock.Object);

        var types = await handler.Handle(query, It.IsAny<CancellationToken>());
        
        Assert.Equal(typesCount, types.Count);
        _cacheHandlerMock.Verify(ch => 
            ch.CacheResponseAsync(requestDto, It.IsAny<object>(), TimeSpan.FromSeconds(TypesTimeToLive)), Times.Once);
    }
    
    [Fact]
    public async Task Handle_WhenHasCachedData_ReturnsTypesFromCache()
    {
        const int typesCount = 4;
        var queryCollection = new QueryCollection();
        var requestDto = new HttpRequestDto("/api/", queryCollection);
        
        var query = new GetAllTypesQuery(requestDto);
        
        var cachedResponse = _fixture.CreateMany<ProductType>(typesCount).ToList();
        _cacheHandlerMock.Setup(ch => ch.GetCachedResponseAsync<IReadOnlyList<ProductType>>(requestDto))
            .ReturnsAsync(cachedResponse);
        
        var handler = new GetAllTypesHandler(
            _loggerMock.Object,
            _cacheConfigMock.Object,
            _typesRepositoryMock.Object,
            _cacheHandlerMock.Object);

        var types = await handler.Handle(query, It.IsAny<CancellationToken>());
        
        Assert.Equal(typesCount, types.Count);
        _typesRepositoryMock.Verify(pr => pr.GetEntitiesAsync(), Times.Never);
        _cacheHandlerMock.Verify(ch => 
            ch.CacheResponseAsync(requestDto, It.IsAny<object>(), TimeSpan.FromSeconds(TypesTimeToLive)), Times.Never);
    }
}