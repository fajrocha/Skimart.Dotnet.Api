using ErrorOr;
using MediatR;
using Skimart.Application.Identity.DTOs;
using Skimart.Application.Identity.Errors;
using Skimart.Application.Identity.Gateways;
using Error = ErrorOr.Error;

namespace Skimart.Application.Identity.Queries.GetCurrentLoggedUser;

public class GetCurrentLoggedUserHandler : IRequestHandler<GetCurrentLoggedUserQuery, ErrorOr<UserDto>>
{
    private readonly IAuthService _authService;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly ITokenService _tokenService;

    public GetCurrentLoggedUserHandler(
        IAuthService authService, 
        ICurrentUserProvider currentUserProvider,
        ITokenService tokenService)
    {
        _authService = authService;
        _currentUserProvider = currentUserProvider;
        _tokenService = tokenService;
    }

    public async Task<ErrorOr<UserDto>> Handle(GetCurrentLoggedUserQuery query, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserProvider.GetCurrentUserFromClaims();
        var user = await _authService.FindUserByEmailAsync(currentUser.Email);
        
        if (user is null) 
            return Error.Failure(description: IdentityErrors.UserNotFoundOnToken);

        return new UserDto(currentUser.Email, currentUser.DisplayName, _tokenService.CreateToken(user)) ;
    }
}