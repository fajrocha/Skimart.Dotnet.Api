using FluentValidation;

namespace Skimart.Application.Orders.Commands.CreateOrder;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(co => co.BasketId).NotEmpty();
        RuleFor(co => co.DeliveryMethodId)
            .GreaterThan(0)
            .LessThanOrEqualTo(int.MaxValue);

        RuleFor(co => co.ShippingAddress).SetValidator(new CreateOrderShippingAddressValidator());
    }
}