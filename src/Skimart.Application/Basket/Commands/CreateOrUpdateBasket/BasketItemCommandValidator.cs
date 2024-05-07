using FluentValidation;

namespace Skimart.Application.Basket.Commands.CreateOrUpdateBasket;

public class BasketItemCommandValidator : AbstractValidator<BasketItemCommand>
{
    public BasketItemCommandValidator()
    {
        RuleFor(basketItem => basketItem.Id).NotNull();
        
        RuleFor(basketItem => basketItem.Price)
            .NotNull()
            .GreaterThan(0)
            .LessThanOrEqualTo(decimal.MaxValue);
        
        RuleFor(basketItem => basketItem.Brand).NotNull().NotEmpty();
        
        RuleFor(basketItem => basketItem.PictureUrl).NotNull().NotEmpty();
        
        RuleFor(basketItem => basketItem.Quantity)
            .NotNull()
            .GreaterThan(0)
            .LessThanOrEqualTo(int.MaxValue);
        
        RuleFor(basketItem => basketItem.Type).NotNull().NotEmpty();
        
        RuleFor(basketItem => basketItem.ProductName).NotNull().NotEmpty();
    }
}