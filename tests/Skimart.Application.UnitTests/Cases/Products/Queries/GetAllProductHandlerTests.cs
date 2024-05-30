using AutoFixture;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Skimart.Application.Cache.Configurations;
using Skimart.Application.Cache.Gateways;
using Skimart.Application.Cases.Shared.Dtos;
using Skimart.Application.Products.Gateways;
using Skimart.Application.Products.Queries.GetAllProducts;
using Skimart.Contracts.Products.Requests;
using Skimart.Domain.Entities.Products;
using Skimart.Mappers;

namespace Skimart.Application.UnitTests.Cases.Products.Queries;

public class GetAllProductHandlerTests
{
    private const int ProductsTimeToLive = 1;
    
    private readonly Mock<ILogger<GetAllProductHandler>> _loggerMock;
    private readonly Mock<IOptions<CacheConfiguration>> _cacheConfigMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ICacheHandler> _cacheHandlerMock;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public GetAllProductHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetAllProductHandler>>();
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
    public async Task Handle_WhenNoCachedResponse_ReturnsProductsFromPersistenceAndCachesIt()
    {
        const int pageSize = 6;
        const int pageIndex = 1;
        const int productCount = 10;
        
        var productParams = _fixture.Build<ProductRequest>()
            .With(p => p.PageIndex, pageIndex)
            .With(p => p.PageSize, pageSize)
            .Create();

        var queryCollection = new QueryCollection();
        var requestDto = new HttpRequestDto("/api/", queryCollection);
        
        var query = new GetAllProductsQuery(productParams, requestDto);
        
        PaginatedDataResponse<ProductResponse>? cachedResponse = null;
        _cacheHandlerMock.Setup(ch => ch.GetCachedResponseAsync<PaginatedDataResponse<ProductResponse>>(requestDto))
            .ReturnsAsync(cachedResponse);

        _productRepositoryMock.Setup(pr => pr.CountAsync(productParams)).ReturnsAsync(productCount);

        var products = _fixture.CreateMany<Product>(productCount).ToList();
        _productRepositoryMock.Setup(pr => pr.GetEntitiesAsync(productParams)).ReturnsAsync(products);

        var handler = new GetAllProductHandler(
            _loggerMock.Object,
            _cacheConfigMock.Object,
            _mapper,
            _productRepositoryMock.Object,
            _cacheHandlerMock.Object);

        var actualProducts = await handler.Handle(query, It.IsAny<CancellationToken>());
        
        Assert.True(actualProducts.PageIndex == pageIndex);
        Assert.True(actualProducts.PageSize == pageSize);
        Assert.True(actualProducts.Data.Count == productCount);
        _cacheHandlerMock.Verify(ch => 
            ch.CacheResponseAsync(requestDto, It.IsAny<object>(), TimeSpan.FromSeconds(ProductsTimeToLive)), Times.Once);
    }
    
    [Fact]
    public async Task Handle_WhenHasCachedResponse_ReturnsProductsFromCache()
    {
        const int pageSize = 6;
        const int pageIndex = 1;
        const int productCount = 10;
        
        var productParams = _fixture.Build<ProductRequest>()
            .With(p => p.PageIndex, pageIndex)
            .With(p => p.PageSize, pageSize)
            .Create();

        var queryCollection = new QueryCollection();
        var requestDto = new HttpRequestDto("/api/", queryCollection);
        
        var query = new GetAllProductsQuery(productParams, requestDto);

        var products = _fixture.CreateMany<ProductResponse>(productCount).ToList();
        var cachedResponse = _fixture.Build<PaginatedDataResponse<ProductResponse>>()
            .With(p => p.PageIndex, pageIndex)
            .With(p => p.PageSize, pageSize)
            .With(p => p.Data, products)
            .Create();
        
        _cacheHandlerMock.Setup(ch => ch.GetCachedResponseAsync<PaginatedDataResponse<ProductResponse>>(requestDto))
            .ReturnsAsync(cachedResponse);

        var handler = new GetAllProductHandler(
            _loggerMock.Object,
            _cacheConfigMock.Object,
            _mapper,
            _productRepositoryMock.Object,
            _cacheHandlerMock.Object);

        var actualProducts = await handler.Handle(query, It.IsAny<CancellationToken>());
        
        Assert.Equal(pageIndex, actualProducts.PageIndex);
        Assert.Equal(pageSize, actualProducts.PageSize);
        Assert.Equal(productCount, actualProducts.Data.Count);
        _productRepositoryMock.Verify(pr => pr.GetEntitiesAsync(productParams), Times.Never);
        _productRepositoryMock.Verify(pr => pr.CountAsync(productParams), Times.Never);
        _cacheHandlerMock.Verify(ch => 
            ch.CacheResponseAsync(requestDto, It.IsAny<object>(), TimeSpan.FromSeconds(ProductsTimeToLive)), Times.Never);
    }
}