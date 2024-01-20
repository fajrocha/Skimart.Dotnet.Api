using FluentResults;
using MediatR;
using Skimart.Application.Abstractions.Auth;
using Skimart.Application.Cases.Auth.Dtos;
using Skimart.Application.Cases.Auth.Errors;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Application.Cases.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<UserDto>>
{
    private readonly IUserManager<AppUser> _userManager;
    private readonly ISignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(
        IUserManager<AppUser> userManager, 
        ISignInManager<AppUser> signInManager, 
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }
    
    public async Task<Result<UserDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null) return Result.Fail(AppIdentityError.UserLoginFailed);

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        return result.Succeeded ? 
            Result.Ok(new UserDto(user.Email, user.DisplayName, _tokenService.CreateToken(user))) :
            Result.Fail(AppIdentityError.UserLoginFailed);
    }
}