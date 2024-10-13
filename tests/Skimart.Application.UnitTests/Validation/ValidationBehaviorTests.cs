using AutoFixture;
using ErrorOr;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using NSubstitute;
using Skimart.Application.Basket.Commands.CreateOrUpdateBasket;
using Skimart.Application.Validation;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.UnitTests.Validation;

public class ValidationBehaviorTests
{
    private readonly RequestHandlerDelegate<ErrorOr<CustomerBasket>> _mockNext;
    private readonly IValidator<CreateOrUpdateBasketCommand> _mockValidator;
    private readonly ValidationBehavior<CreateOrUpdateBasketCommand, ErrorOr<CustomerBasket>> _sut;
    private readonly Fixture _fixture;

    public ValidationBehaviorTests()
    {
        _fixture = new Fixture();
        _mockNext = Substitute.For<RequestHandlerDelegate<ErrorOr<CustomerBasket>>>();
        _mockValidator = Substitute.For<IValidator<CreateOrUpdateBasketCommand>>();
        _sut = new ValidationBehavior<CreateOrUpdateBasketCommand, ErrorOr<CustomerBasket>>(_mockValidator);
    }

    [Fact]
    public async Task Handle_WhenValidationSucceeds_ShouldInvokeNext()
    {
        var command = _fixture.Create<CreateOrUpdateBasketCommand>();
        var basket = _fixture.Create<CustomerBasket>();
        var validationSuccess = new ValidationResult();
        
        _mockNext.Invoke().Returns(basket);
        _mockValidator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(validationSuccess);

        var result = await _sut.Handle(command, _mockNext, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(basket);
    }
    
    [Fact]
    public async Task Handle_WhenValidationFails_ShouldInvokeNext()
    {
        // Arrange:
        var command = _fixture.Create<CreateOrUpdateBasketCommand>();
        var basket = _fixture.Create<CustomerBasket>();
        var validationErrors = BuildValidationFailures(5);
        var propertiesNames = validationErrors.Select(ve => ve.PropertyName).ToList();
        var errorMessages = validationErrors.Select(ve => ve.ErrorMessage).ToList();
        var validationFailure = new ValidationResult(validationErrors);
        
        _mockNext.Invoke().Returns(basket);
        _mockValidator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(validationFailure);

        // Act
        var result = await _sut.Handle(command, _mockNext, default);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().AllSatisfy(e => e.Type.Should().Be(ErrorType.Validation));
        
        var resultPropertyNames = result.Errors.Select(e => e.Code).ToList();
        var resultErrorMessages = result.Errors.Select(e => e.Description).ToList();
        resultPropertyNames.Should().BeEquivalentTo(propertiesNames);
        resultErrorMessages.Should().BeEquivalentTo(errorMessages);
    }

    private List<ValidationFailure> BuildValidationFailures(int upTo)
    {
        var validationsErrorsCollection = new List<ValidationFailure>();
        var validationErrors = new Random().Next(1, upTo + 1);

        for (var i = 0; i < validationErrors; i++)
        {
            var propertyName = _fixture.Create<string>();
            var errorMessage = _fixture.Create<string>();
            var validationError = new ValidationFailure(propertyName, errorMessage);
            
            validationsErrorsCollection.Add(validationError);
        }

        return validationsErrorsCollection;
    }
}