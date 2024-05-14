using ErrorOr;
using MediatR;
using Skimart.Application.Identity.DTOs;
using Skimart.Application.Identity.Errors;
using Skimart.Application.Identity.Gateways;
using Error = ErrorOr.Error;

namespace Skimart.Application.Identity.Queries.GetCurrentLoggedUser;

public class GetCurrentLoggedUserHandler : IRequestHandler<GetCurrentLoggedUserQuery, ErrorOr<CurrentUserDto>>
{
    private readonly IAuthService _authService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public GetCurrentLoggedUserHandler(IAuthService authService, ICurrentUserProvider currentUserProvider)
    {
        _authService = authService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<ErrorOr<CurrentUserDto>> Handle(GetCurrentLoggedUserQuery query, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserProvider.GetCurrentUserFromClaims();
        var user = await _authService.FindUserByEmailAsync(currentUser.Email);

        if (user is null) 
            return Error.Failure(description: IdentityErrors.UserFromTokenNotFound);

        return currentUser;
    }
}