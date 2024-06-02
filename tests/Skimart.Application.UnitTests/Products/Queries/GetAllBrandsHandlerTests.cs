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
    private readonly IProductBrandRepository _mockRepository;
    private readonly GetAllBrandsHandler _sut;
    private readonly Fixture _fixture;

    public GetAllBrandsHandlerTests()
    {
        _fixture = new Fixture();
        var mockLogger = Substitute.For<ILogger<GetAllBrandsHandler>>();
        _mockRepository = Substitute.For<IProductBrandRepository>();
        _sut = new GetAllBrandsHandler(mockLogger, _mockRepository);
    }

    [Fact]
    public async Task Handle_WhenCalled_ShouldReturnBrands()
    {
        var expectedBrands = _fixture.CreateMany<ProductBrand>().ToList();
        _mockRepository.GetEntitiesAsync().Returns(expectedBrands);

        var query = new GetAllBrandsQuery();
        var result = await _sut.Handle(query, default);

        result.Should().BeEquivalentTo(expectedBrands);
    }
}