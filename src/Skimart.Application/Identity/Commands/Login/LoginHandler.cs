using ErrorOr;
using MediatR;
using Skimart.Application.Identity.DTOs;
using Skimart.Application.Identity.Errors;
using Skimart.Application.Identity.Gateways;

namespace Skimart.Application.Identity.Commands.Login;

public class LoginHandler : IRequestHandler<LoginCommand, ErrorOr<UserDto>>
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;

    public LoginHandler(IAuthService authService, ITokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }
    
    public async Task<ErrorOr<UserDto>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _authService.FindUserByEmailAsync(command.Email);

        if (user is null) 
            return Error.Unauthorized(description: IdentityErrors.LoginFailed);

        var result = await _authService.CheckPasswordAsync(user, command.Password);

        return result ? 
            new UserDto(user.Email, user.DisplayName, _tokenService.CreateToken(user)) :
            Error.Unauthorized(description: IdentityErrors.LoginFailed);
    }
}