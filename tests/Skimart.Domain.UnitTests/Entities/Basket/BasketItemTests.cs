using AutoFixture;
using FluentAssertions;
using Skimart.Domain.Entities.Basket;
using Skimart.Domain.Entities.Products;

namespace Skimart.Domain.UnitTests.Entities.Basket;

public class BasketItemTests
{
    private readonly Fixture _fixture;
    private readonly BasketItem _sut;

    public BasketItemTests()
    {
        _fixture = new Fixture();
        _sut = _fixture.Create<BasketItem>();
    }
    
    [Fact]
    public void VerifyAgainstStoredPrice_WhenPriceIsTheSame_ShouldNotUpdatePrice()
    {

        var product = _fixture.Build<Product>().With(product => product.Price, _sut.Price).Create();
        
        _sut.VerifyAgainstStoredPrice(product);

        _sut.Price.Should().Be(product.Price);
    }
    
    [Fact]
    public void VerifyAgainstStoredPrice_WhenPriceIsDifferentTheSame_ShouldUpdatePrice()
    {
        var productPrice = _sut.Price + new Random().Next(1, 10);
        var product = _fixture.Build<Product>().With(product => product.Price, productPrice).Create();
        
        _sut.VerifyAgainstStoredPrice(product);

        _sut.Price.Should().Be(product.Price);
    }
}