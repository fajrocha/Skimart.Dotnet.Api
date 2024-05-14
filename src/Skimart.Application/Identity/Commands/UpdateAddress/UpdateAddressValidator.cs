using FluentValidation;

namespace Skimart.Application.Identity.Commands.UpdateAddress;

public class UpdateAddressValidator : AbstractValidator<UpdateAddressCommand>
{
    public UpdateAddressValidator()
    {
        RuleFor(a => a.FirstName).NotEmpty();
        RuleFor(a => a.LastName).NotEmpty();
        RuleFor(a => a.ZipCode).NotEmpty();
        RuleFor(a => a.Province).NotEmpty();
        RuleFor(a => a.City).NotEmpty();
        RuleFor(a => a.Street).NotEmpty();
    }
}