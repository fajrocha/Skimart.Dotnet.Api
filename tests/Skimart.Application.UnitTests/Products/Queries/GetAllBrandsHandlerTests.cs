using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Skimart.Application.Products.Gateways;
using Skimart.Application.Products.Queries.GetAllProductBrands;
using Skimart.Domain.Entities.Products;

namespace Skimart.Application.UnitTests.Products.Queries;

public class GetAllBrandsHandlerTests
{
    private readonly IProductBrandRepository _repositoryMock;
    private readonly GetAllBrandsHandler _sut;
    private readonly Fixture _fixture;

    public GetAllBrandsHandlerTests()
    {
        _fixture = new Fixture();
        var loggerMock = Substitute.For<ILogger<GetAllBrandsHandler>>();
        _repositoryMock = Substitute.For<IProductBrandRepository>();
        _sut = new GetAllBrandsHandler(loggerMock, _repositoryMock);
    }

    [Fact]
    public async Task Handle_WhenBrandsAreReturnedFromRepository_ShouldReturnBrandsCollection()
    {
        var expectedBrands = _fixture.CreateMany<ProductBrand>().ToList();
        _repositoryMock.GetEntitiesAsync().Returns(expectedBrands);

        var query = new GetAllBrandsQuery();
        var result = await _sut.Handle(query, default);

        result.Should().BeEquivalentTo(expectedBrands);
    }
    
    [Fact]
    public async Task Handle_WhenNoBrandsAreReturnedFromRepository_ShouldReturnEmptyCollection()
    {
        var expectedBrands = new List<ProductBrand>();
        _repositoryMock.GetEntitiesAsync().Returns(expectedBrands);

        var query = new GetAllBrandsQuery();
        var result = await _sut.Handle(query, default);

        result.Should().BeEmpty();
    }
}