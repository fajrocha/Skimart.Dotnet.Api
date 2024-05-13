using FluentValidation;

namespace Skimart.Application.Identity.Commands.Login;

public class LoginCommandValidators : AbstractValidator<LoginCommand>
{
    public LoginCommandValidators()
    {
        RuleFor(lc => lc.Email).NotEmpty();
        RuleFor(lc => lc.Password).NotEmpty();
    }   
}