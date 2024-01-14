using System.Runtime.CompilerServices;
using AutoFixture;
using Domain.Entities.Product;
using MapsterMapper;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Skimart.Application.Abstractions.Memory.Cache;
using Skimart.Application.Abstractions.Persistence.Repositories.StoreProduct;
using Skimart.Application.Cases.Products.Dtos;
using Skimart.Application.Cases.Products.Queries.GetAllProducts;
using Skimart.Application.Cases.Shared.Dtos;
using Skimart.Application.Cases.Shared.Vms;
using Skimart.Application.Configurations.Memory;
using Skimart.Application.Mappers;

namespace Skimart.Application.UnitTests.Cases.Products.Queries;

public class GetAllProductHandlerTests
{
    private const int ProductsTimeToLive = 1;
    
    private readonly Mock<ILogger<GetAllProductHandler>> _loggerMock;
    private readonly Mock<IOptions<CacheConfig>> _cacheConfigMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ICacheHandler> _cacheHandlerMock;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public GetAllProductHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetAllProductHandler>>();
        _cacheConfigMock = new Mock<IOptions<CacheConfig>>();
        
        _cacheConfigMock.Setup(s => s.Value).Returns(new CacheConfig
        {
            ProductsTimeToLive = ProductsTimeToLive,
        });
        
        _productRepositoryMock = new Mock<IProductRepository>();
        _cacheHandlerMock = new Mock<ICacheHandler>();
        _mapper = AppMapper.GetMapper();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_WhenNoCachedResponse_ReturnsProductsFromPersistenceAndCachesIt()
    {
        const int pageSize = 6;
        const int pageIndex = 1;
        const int productCount = 10;
        
        var productParams = _fixture.Build<ProductParams>()
            .With(p => p.PageIndex, pageIndex)
            .With(p => p.PageSize, pageSize)
            .Create();

        var queryCollection = new QueryCollection();
        var requestDto = new HttpRequestDto("/api/", queryCollection);
        
        var query = new GetAllProductsQuery(productParams, requestDto);
        
        PaginatedDataVm<ProductToReturnDto>? cachedResponse = null;
        _cacheHandlerMock.Setup(ch => ch.GetCachedResponseAsync<PaginatedDataVm<ProductToReturnDto>>(requestDto))
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
        
        var productParams = _fixture.Build<ProductParams>()
            .With(p => p.PageIndex, pageIndex)
            .With(p => p.PageSize, pageSize)
            .Create();

        var queryCollection = new QueryCollection();
        var requestDto = new HttpRequestDto("/api/", queryCollection);
        
        var query = new GetAllProductsQuery(productParams, requestDto);

        var products = _fixture.CreateMany<ProductToReturnDto>(productCount).ToList();
        var cachedResponse = _fixture.Build<PaginatedDataVm<ProductToReturnDto>>()
            .With(p => p.PageIndex, pageIndex)
            .With(p => p.PageSize, pageSize)
            .With(p => p.Data, products)
            .Create();
        
        _cacheHandlerMock.Setup(ch => ch.GetCachedResponseAsync<PaginatedDataVm<ProductToReturnDto>>(requestDto))
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