using FluentValidation;

namespace Skimart.Application.Orders.Commands.CreateOrder;

public class CreateOrderShippingAddressValidator : AbstractValidator<CreateOrderShippingAddressCommand>
{
    public CreateOrderShippingAddressValidator()
    {
        RuleFor(sa => sa.FirstName).NotEmpty();
        RuleFor(sa => sa.LastName).NotEmpty();
        RuleFor(sa => sa.Street).NotEmpty();
        RuleFor(sa => sa.City).NotEmpty();
        RuleFor(sa => sa.Province).NotEmpty();
        RuleFor(sa => sa.ZipCode).NotEmpty();
    }
}