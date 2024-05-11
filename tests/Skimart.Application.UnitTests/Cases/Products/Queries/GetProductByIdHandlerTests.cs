using AutoFixture;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Skimart.Application.Cases.Shared.Dtos;
using Skimart.Application.Configurations.Memory;
using Skimart.Application.Gateways.Memory.Cache;
using Skimart.Application.Gateways.Persistence.Repositories.StoreProduct;
using Skimart.Application.Products.Queries.GetProductById;
using Skimart.Domain.Entities.Products;
using Skimart.Mappers;

namespace Skimart.Application.UnitTests.Cases.Products.Queries;

public class GetProductByIdHandlerTests
{
    private const int ProductsTimeToLive = 1;
    
    private readonly Mock<ILogger<GetProductByIdHandler>> _loggerMock;
    private readonly Mock<IOptions<CacheConfiguration>> _cacheConfigMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ICacheHandler> _cacheHandlerMock;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public GetProductByIdHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetProductByIdHandler>>();
        _cacheConfigMock = new Mock<IOptions<CacheConfiguration>>();
        _cacheConfigMock.Setup(s => s.Value).Returns(new CacheConfiguration
        {
            TimeToLiveSecs = ProductsTimeToLive,
        });
        
        _productRepositoryMock = new Mock<IProductRepository>();
        _cacheHandlerMock = new Mock<ICacheHandler>();
        _mapper = MapperBootstrap.GetMapper();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_WhenNoCachedResponse_ReturnsProductFromPersistenceAndCachesIt()
    {
        var queryCollection = new QueryCollection();
        var requestDto = new HttpRequestDto("/api/", queryCollection);
        const int id = 1;

        var query = new GetProductByIdQuery(id, requestDto);
        
        ProductResponse? cachedProduct = null;
        _cacheHandlerMock.Setup(ch => ch.GetCachedResponseAsync<ProductResponse>(requestDto))
            .ReturnsAsync(cachedProduct);
        
        var product = _fixture.Build<Product>().With(p => p.Id, id).Create();
        _productRepositoryMock.Setup(pr => pr.GetEntityByIdAsync(id)).ReturnsAsync(product);
        
        var handler = new GetProductByIdHandler(
            _loggerMock.Object,
            _cacheConfigMock.Object,
            _mapper,
            _productRepositoryMock.Object,
            _cacheHandlerMock.Object);
        
        var result = await handler.Handle(query, It.IsAny<CancellationToken>());
        var actualProduct = result.Value;
        
        Assert.True(result.IsSuccess);
        Assert.Equal(id, actualProduct.Id);
        _cacheHandlerMock.Verify(ch => 
            ch.CacheResponseAsync(requestDto, It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_WhenCachedResponse_ReturnsProductFromCache()
    {
        var queryCollection = new QueryCollection();
        var requestDto = new HttpRequestDto("/api/", queryCollection);
        const int id = 1;

        var query = new GetProductByIdQuery(id, requestDto);
        
        var cachedProduct = _fixture.Build<ProductResponse>().With(p => p.Id, id).Create();
        _cacheHandlerMock.Setup(ch => ch.GetCachedResponseAsync<ProductResponse>(requestDto))
            .ReturnsAsync(cachedProduct);
        
        var handler = new GetProductByIdHandler(
            _loggerMock.Object,
            _cacheConfigMock.Object,
            _mapper,
            _productRepositoryMock.Object,
            _cacheHandlerMock.Object);
        
        var result = await handler.Handle(query, It.IsAny<CancellationToken>());
        var actualProduct = result.Value;
        
        Assert.True(result.IsSuccess);
        Assert.Equal(id, actualProduct.Id);
        _productRepositoryMock.Verify(pr => pr.GetEntityByIdAsync(id), Times.Never);
        _cacheHandlerMock.Verify(ch => 
            ch.CacheResponseAsync(requestDto, It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_WhenProductNotFound_ReturnsFailedResult()
    {
        var queryCollection = new QueryCollection();
        var requestDto = new HttpRequestDto("/api/", queryCollection);
        const int id = 1;

        var query = new GetProductByIdQuery(id, requestDto);
        
        ProductResponse? cachedResponse = null;
        _cacheHandlerMock.Setup(ch => ch.GetCachedResponseAsync<ProductResponse>(requestDto))
            .ReturnsAsync(cachedResponse);
        
        Product? product = null;
        _productRepositoryMock.Setup(pr => pr.GetEntityByIdAsync(id)).ReturnsAsync(product);
        
        var handler = new GetProductByIdHandler(
            _loggerMock.Object,
            _cacheConfigMock.Object,
            _mapper,
            _productRepositoryMock.Object,
            _cacheHandlerMock.Object);
        
        var result = await handler.Handle(query, It.IsAny<CancellationToken>());
        
        Assert.True(result.IsFailed);
        Assert.Contains(ProductError.NotFound.Message, result.GetReasonsAsCollection());
        _cacheHandlerMock.Verify(ch 
            => ch.CacheResponseAsync(requestDto, It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
    }
}