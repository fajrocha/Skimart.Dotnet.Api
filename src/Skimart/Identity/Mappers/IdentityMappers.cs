using Skimart.Application.Identity.Commands.Login;
using Skimart.Application.Identity.DTOs;
using Skimart.Contracts.Identity.Requests;
using Skimart.Contracts.Identity.Responses;

namespace Skimart.Identity.Mappers;

public static class IdentityMappers
{
    public static LoginCommand ToCommand(this LoginRequest loginRequest)
    {
        return new LoginCommand(loginRequest.Email, loginRequest.Password);
    }
    
    public static LoginResponse ToResponse(this UserDto userDto)
    {
        return new LoginResponse(userDto.Email, userDto.DisplayName, userDto.Token);
    }
}