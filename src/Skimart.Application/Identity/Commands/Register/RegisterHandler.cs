using ErrorOr;
using MediatR;
using Skimart.Application.Identity.DTOs;
using Skimart.Application.Identity.Errors;
using Skimart.Application.Identity.Gateways;
using Skimart.Application.Identity.Mappers;

namespace Skimart.Application.Identity.Commands.Register;

public class RegisterHandler :  IRequestHandler<RegisterCommand, ErrorOr<UserDto>>
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;

    public RegisterHandler(IAuthService authService, ITokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }
    
    public async Task<ErrorOr<UserDto>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var email = command.Email;
        var userInStore = await _authService.FindUserByEmailAsync(email);

        if (userInStore is not null)
            return Error.Conflict(description: IdentityErrors.UserAlreadyExists);

        var userToCreate = command.ToUser();
        
        var result = await _authService.CreateUserAsync(userToCreate, command.Password);

        return result ? 
            new UserDto(userToCreate.Email, userToCreate.DisplayName, _tokenService.CreateToken(userToCreate)) :
            Error.Conflict(description: IdentityErrors.RegistrationFailed);
    }
}