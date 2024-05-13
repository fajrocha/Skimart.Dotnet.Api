using FluentResults;
using MapsterMapper;
using MediatR;
using Skimart.Application.Abstractions.Auth;
using Skimart.Application.Cases.Auth.Errors;
using Skimart.Application.Cases.Shared.Handlers;
using Skimart.Application.Identity.DTOs;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Application.Identity.Commands.Register;

public class RegisterHandler :  BaseAuthHandler,IRequestHandler<RegisterCommand, Result<UserDto>>
{
    private readonly IMapper _mapper;

    public RegisterHandler(IAuthService authService, ITokenService tokenService, IMapper mapper) 
        : base(authService, tokenService)
    {
        _mapper = mapper;
    }
    
    public async Task<Result<UserDto>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var existingUser = await _authService.FindUserByEmailAsync(command.Email);

        if (existingUser is not null)
            return Result.Fail(AppIdentityError.UserAlreadyExists);
        
        var userToCreate = _mapper.Map<AppUser>(command);
        
        var result = await _authService.CreateUserAsync(userToCreate, command.Password);

        return result ? 
            Result.Ok(new UserDto(userToCreate.Email, userToCreate.DisplayName, _tokenService.CreateToken(userToCreate))) :
            Result.Fail(AppIdentityError.UserRegistrationFailed);
    }
}