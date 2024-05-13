using ErrorOr;
using MediatR;
using Skimart.Application.Abstractions.Auth;
using Skimart.Application.Cases.Shared.Handlers;
using Skimart.Application.Identity.DTOs;

namespace Skimart.Application.Identity.Commands.Login;

public class LoginHandler : BaseAuthHandler, IRequestHandler<LoginCommand, ErrorOr<UserDto>>
{
    public LoginHandler(IAuthService authService, ITokenService tokenService) : base(authService, tokenService)
    {
    }
    
    public async Task<ErrorOr<UserDto>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _authService.FindUserByEmailAsync(command.Email);

        if (user is null) 
            return Error.Unauthorized(description: LoginErrors.LoginFailed);

        var result = await _authService.CheckPasswordAsync(user, command.Password);

        return result ? 
            new UserDto(user.Email, user.DisplayName, _tokenService.CreateToken(user)) :
            Error.Unauthorized(description: LoginErrors.LoginFailed);
    }
}