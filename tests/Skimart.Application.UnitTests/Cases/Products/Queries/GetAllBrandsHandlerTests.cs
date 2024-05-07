using AutoFixture;
using Domain.Entities.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Skimart.Application.Abstractions.Memory.Cache;
using Skimart.Application.Abstractions.Persistence.Repositories.StoreProduct;
using Skimart.Application.Cases.Products.Queries.GetAllProductBrands;
using Skimart.Application.Cases.Shared.Dtos;
using Skimart.Application.Configurations.Memory;

namespace Skimart.Application.UnitTests.Cases.Products.Queries;

public class GetAllBrandsHandlerTests
{
    private const int BrandsTimeToLive = 1;
    
    private readonly Mock<ILogger<GetAllBrandsHandler>> _loggerMock;
    private readonly Mock<IOptions<CacheConfig>> _cacheConfigMock;
    private readonly Mock<IProductBrandRepository> _typesRepositoryMock;
    private readonly Mock<ICacheHandler> _cacheHandlerMock;
    private readonly Fixture _fixture;
    
    public GetAllBrandsHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetAllBrandsHandler>>();
        _cacheConfigMock = new Mock<IOptions<CacheConfig>>();
        
        _cacheConfigMock.Setup(s => s.Value).Returns(new CacheConfig
        {
            BrandsTimeToLive = BrandsTimeToLive,
        });
        
        _typesRepositoryMock = new Mock<IProductBrandRepository>();
        _cacheHandlerMock = new Mock<ICacheHandler>();
        _fixture = new Fixture();
    }
    
    [Fact]
    public async Task Handle_WhenNoCachedData_ReturnsBrandsFromPersistenceAndCachesIt()
    {
        const int brandsCount = 4;
        var queryCollection = new QueryCollection();
        var requestDto = new HttpRequestDto("/api/", queryCollection);
        
        var query = new GetAllBrandsQuery(requestDto);
        
        IReadOnlyList<ProductBrand>? cachedResponse = null;
        _cacheHandlerMock.Setup(ch => ch.GetCachedResponseAsync<IReadOnlyList<ProductBrand>>(requestDto))
            .ReturnsAsync(cachedResponse);
        
        var typesStored = _fixture.CreateMany<ProductBrand>(brandsCount).ToList();
        _typesRepositoryMock.Setup(tr => tr.GetEntitiesAsync()).ReturnsAsync(typesStored);
        
        var handler = new GetAllBrandsHandler(
            _loggerMock.Object,
            _cacheConfigMock.Object,
            _typesRepositoryMock.Object,
            _cacheHandlerMock.Object);

        var types = await handler.Handle(query, It.IsAny<CancellationToken>());
        
        Assert.Equal(brandsCount, types.Count);
        _cacheHandlerMock.Verify(ch => 
            ch.CacheResponseAsync(requestDto, It.IsAny<object>(), TimeSpan.FromSeconds(BrandsTimeToLive)), Times.Once);
    }
    
    [Fact]
    public async Task Handle_WhenHasCachedData_ReturnsTypesFromCache()
    {
        const int brandsCount = 4;
        var queryCollection = new QueryCollection();
        var requestDto = new HttpRequestDto("/api/", queryCollection);
        
        var query = new GetAllBrandsQuery(requestDto);
        
        var cachedResponse = _fixture.CreateMany<ProductBrand>(brandsCount).ToList();
        _cacheHandlerMock.Setup(ch => ch.GetCachedResponseAsync<IReadOnlyList<ProductBrand>>(requestDto))
            .ReturnsAsync(cachedResponse);
        
        var handler = new GetAllBrandsHandler(
            _loggerMock.Object,
            _cacheConfigMock.Object,
            _typesRepositoryMock.Object,
            _cacheHandlerMock.Object);

        var types = await handler.Handle(query, It.IsAny<CancellationToken>());
        
        Assert.Equal(brandsCount, types.Count);
        _typesRepositoryMock.Verify(pr => pr.GetEntitiesAsync(), Times.Never);
        _cacheHandlerMock.Verify(ch => 
            ch.CacheResponseAsync(requestDto, It.IsAny<object>(), TimeSpan.FromSeconds(BrandsTimeToLive)), Times.Never);
    }
}