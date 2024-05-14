using ErrorOr;
using MediatR;
using Skimart.Application.Identity.Errors;
using Skimart.Application.Identity.Gateways;
using Skimart.Domain.Entities.Auth;
using Error = ErrorOr.Error;

namespace Skimart.Application.Identity.Commands.UpdateAddress;

public class UpdateAddressHandler : IRequestHandler<UpdateAddressCommand, ErrorOr<Address>>
{
    private readonly IAuthService _authService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public UpdateAddressHandler(IAuthService authService, ICurrentUserProvider currentUserProvider)
    {
        _authService = authService;
        _currentUserProvider = currentUserProvider;
    }
    
    public async Task<ErrorOr<Address>> Handle(UpdateAddressCommand command, CancellationToken cancellationToken)
    {
        var userFromToken = _currentUserProvider.GetCurrentUserFromClaims();
        var user = await _authService.FindUserWithAddressByEmail(userFromToken.Email);

        if (user is null)
            return Error.Failure(description: IdentityErrors.UserFromTokenNotFound);

        UpdateAddress(user.Address, command);

        var result = await _authService.UpdateAddressAsync(user);
        
        return result ? user.Address : Error.Failure(IdentityErrors.UpdatingAddressFailed);
    }

    private static void UpdateAddress(Address address, UpdateAddressCommand updateCommand)
    {
        address.FirstName = updateCommand.FirstName;
        address.LastName = updateCommand.LastName;
        address.Street = updateCommand.Street;
        address.City = updateCommand.City;
        address.Province = updateCommand.Province;
        address.ZipCode = updateCommand.ZipCode;
    }
}