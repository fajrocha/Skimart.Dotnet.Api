using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Skimart.Application.Products.Gateways;
using Skimart.Application.Products.Queries.GetAllProductBrands;
using Skimart.Application.Products.Queries.GetAllProductTypes;
using Skimart.Domain.Entities.Products;

namespace Skimart.Application.UnitTests.Products.Queries;

public class GetAllTypesHandlerTests
{
    private readonly IProductTypeRepository _mockRepository;
    private readonly GetAllTypesHandler _sut;
    private readonly Fixture _fixture;

    public GetAllTypesHandlerTests()
    {
        _fixture = new Fixture();
        var mockLogger = Substitute.For<ILogger<GetAllTypesHandler>>();
        _mockRepository = Substitute.For<IProductTypeRepository>();
        _sut = new GetAllTypesHandler(mockLogger, _mockRepository);
    }

    [Fact]
    public async Task Handle_WhenCalled_ShouldReturnTypes()
    {
        var expectedBrands = _fixture.CreateMany<ProductType>().ToList();
        _mockRepository.GetEntitiesAsync().Returns(expectedBrands);

        var query = new GetAllTypesQuery();
        var result = await _sut.Handle(query, default);

        result.Should().BeEquivalentTo(expectedBrands);
    }
}