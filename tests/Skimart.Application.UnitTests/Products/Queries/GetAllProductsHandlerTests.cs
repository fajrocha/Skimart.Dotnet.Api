﻿using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Skimart.Application.Products.Gateways;
using Skimart.Application.Products.Queries.GetAllProducts;
using Skimart.Domain.Entities.Products;

namespace Skimart.Application.UnitTests.Products.Queries;

public class GetAllProductsHandlerTests
{
    private readonly Fixture _fixture;
    private readonly IProductRepository _mockRepository;
    private readonly GetAllProductHandler _sut;

    public GetAllProductsHandlerTests()
    {
        _fixture = new Fixture();
        var mockLogger = Substitute.For<ILogger<GetAllProductHandler>>();
        _mockRepository = Substitute.For<IProductRepository>();
        _sut = new GetAllProductHandler(mockLogger, _mockRepository);
    }

    [Fact]
    public async Task Handle_WhenRepositoryReturnsData_ReturnsDataWithCount()
    {
        var expectedProducts = _fixture.CreateMany<Product>().ToList();
        var productsCount = expectedProducts.Count;

        var query = new GetAllProductsQuery(
            PageIndex: 1, 
            PageSize: productsCount,
            BrandId: null,
            TypeId: null,
            Sort: null,
            Search: string.Empty);

        _mockRepository.GetEntitiesAsync(query).Returns(expectedProducts);
        _mockRepository.CountAsync(query).Returns(productsCount);

        var result = await _sut.Handle(query, default);

        result.Should().BeEquivalentTo(new ProductsResponseDto(productsCount, expectedProducts));
    }
    
    [Fact]
    public async Task Handle_WhenRepositoryReturnsNoData_ReturnsDataWithCountAtZero()
    {
        var expectedProducts = new List<Product>();
        var productsCount = expectedProducts.Count;

        var query = new GetAllProductsQuery(
            PageIndex: 1, 
            PageSize: productsCount,
            BrandId: null,
            TypeId: null,
            Sort: null,
            Search: string.Empty);

        _mockRepository.GetEntitiesAsync(query).Returns(expectedProducts);
        _mockRepository.CountAsync(query).Returns(productsCount);

        var result = await _sut.Handle(query, default);

        result.Should().BeEquivalentTo(new ProductsResponseDto(productsCount, expectedProducts));
    }
}