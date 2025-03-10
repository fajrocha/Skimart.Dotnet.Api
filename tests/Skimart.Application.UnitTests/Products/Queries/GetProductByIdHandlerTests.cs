﻿using AutoFixture;
using ErrorOr;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Skimart.Application.Products.Errors;
using Skimart.Application.Products.Gateways;
using Skimart.Application.Products.Queries.GetAllProductTypes;
using Skimart.Application.Products.Queries.GetProductById;
using Skimart.Domain.Entities.Products;

namespace Skimart.Application.UnitTests.Products.Queries;

public class GetProductByIdHandlerTests
{
    private readonly Fixture _fixture;
    private readonly IProductRepository _mockRepository;
    private readonly GetProductByIdHandler _sut;

    public GetProductByIdHandlerTests()
    {
        _fixture = new Fixture();
        var mockLogger = Substitute.For<ILogger<GetProductByIdHandler>>();
        _mockRepository = Substitute.For<IProductRepository>();
        _sut = new GetProductByIdHandler(mockLogger, _mockRepository);
    }

    [Fact]
    public async Task Handle_WhenProductExists_ReturnsProductWithRequestedId()
    {
        var product = _fixture.Create<Product>();
        var productId = product.Id;

        _mockRepository.GetEntityByIdAsync(productId).Returns(product);
        var query = new GetProductByIdQuery(productId);

        var result = await _sut.Handle(query, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(product);
    }
    
    [Fact]
    public async Task Handle_WhenProductDoesNotExist_ReturnsNotFoundError()
    {
        Product? product = null;
        var productId = 1;
        _mockRepository.GetEntityByIdAsync(productId).Returns(product);
        
        var query = new GetProductByIdQuery(productId);
        var result = await _sut.Handle(query, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Description.Should().Be(ProductErrors.ProductNotFound);
    }
}