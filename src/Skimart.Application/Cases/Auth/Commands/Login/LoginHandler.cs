using FluentResults;
using MediatR;
using Skimart.Application.Abstractions.Auth;
using Skimart.Application.Cases.Auth.Dtos;
using Skimart.Application.Cases.Auth.Errors;
using Skimart.Application.Cases.Shared.Handlers;

namespace Skimart.Application.Cases.Auth.Commands.Login;

public class LoginHandler : BaseAuthHandler, IRequestHandler<LoginCommand, Result<UserDto>>
{
    public LoginHandler(IAuthService authService, ITokenService tokenService) : base(authService, tokenService)
    {
    }
    
    public async Task<Result<UserDto>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _authService.FindUserByEmailAsync(command.Email);

        if (user is null) 
            return Result.Fail(AppIdentityError.UserLoginFailed);

        var result = await _authService.CheckPasswordAsync(user, command.Password);

        return result ? 
            Result.Ok(new UserDto(user.Email, user.DisplayName, _tokenService.CreateToken(user))) :
            Result.Fail(AppIdentityError.UserLoginFailed);
    }
}