using FluentValidation;

namespace Skimart.Application.Basket.Commands.CreateOrUpdateBasket;

public class CreateOrUpdateBasketCommandValidator : AbstractValidator<CreateOrUpdateBasketCommand>
{
    public CreateOrUpdateBasketCommandValidator()
    {
        RuleFor(basket => basket.Id)
            .NotNull()
            .NotEmpty();
        
        RuleFor(basket => basket.Items)
            .NotEmpty()
            .NotNull();

        RuleForEach(basket => basket.Items).SetValidator(new BasketItemCommandValidator());
    }
}