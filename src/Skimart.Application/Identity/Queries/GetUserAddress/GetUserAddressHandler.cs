using ErrorOr;
using FluentResults;
using MediatR;
using Skimart.Application.Identity.DTOs;
using Skimart.Application.Identity.Errors;
using Skimart.Application.Identity.Gateways;
using Skimart.Domain.Entities.Auth;
using Error = ErrorOr.Error;

namespace Skimart.Application.Identity.Queries.GetUserAddress;

public class GetUserAddressHandler : IRequestHandler<GetUserAddressQuery, ErrorOr<Address>>
{
    private readonly IAuthService _authService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public GetUserAddressHandler(IAuthService authService, ICurrentUserProvider currentUserProvider)
    {
        _authService = authService;
        _currentUserProvider = currentUserProvider;
    }
    
    public async Task<ErrorOr<Address>> Handle(GetUserAddressQuery query, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserProvider.GetCurrentUserFromClaims();
        
        var user = await _authService.FindUserWithAddressByEmail(currentUser.Email);

        if (user is null)
            return Error.Failure(description: IdentityErrors.UserFromTokenNotFound);

        return user.Address;
    }
}