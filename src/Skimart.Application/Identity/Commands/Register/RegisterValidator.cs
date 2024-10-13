using FluentValidation;
using Skimart.Domain;

namespace Skimart.Application.Identity.Commands.Register;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(rc => rc.DisplayName).NotEmpty();
        RuleFor(rc => rc.Email).NotEmpty().EmailAddress();
        RuleFor(rc => rc.Password).NotEmpty();
            
        RuleFor(rc => rc.Password)
            .Matches(AppConstants.Pwd.Regex)
            .WithMessage("Password must have one uppercase one lowercase, one number, one " +
                         "non-alphanumeric symbol and at least 6 characters");
    }
}