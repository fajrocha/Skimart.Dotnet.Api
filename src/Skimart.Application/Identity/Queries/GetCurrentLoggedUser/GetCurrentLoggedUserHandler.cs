using FluentResults;
using MediatR;
using Skimart.Application.Abstractions.Auth;
using Skimart.Application.Cases.Auth.Errors;
using Skimart.Application.Cases.Shared.Handlers;
using Skimart.Application.Identity.DTOs;

namespace Skimart.Application.Identity.Queries.GetCurrentLoggedUser;

public class GetCurrentLoggedUserHandler : BaseAuthHandler, IRequestHandler<GetCurrentLoggedUserQuery, Result<UserDto>>
{
    public GetCurrentLoggedUserHandler(IAuthService authService, ITokenService tokenService) 
        : base(authService, tokenService)
    {
    }

    public async Task<Result<UserDto>> Handle(GetCurrentLoggedUserQuery query, CancellationToken cancellationToken)
    {
        var user = await _authService.FindByEmailFromClaims(query.Claims);

        if (user is null) 
            return Result.Fail(AppIdentityError.NoLoggedUser);

        var userToReturn = new UserDto(user.Email, user.DisplayName, _tokenService.CreateToken(user));
        
        return Result.Ok(userToReturn);
    }
}