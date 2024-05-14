using Skimart.Application.Identity.Commands.Login;
using Skimart.Application.Identity.Commands.Register;
using Skimart.Application.Identity.Commands.UpdateAddress;
using Skimart.Application.Identity.DTOs;
using Skimart.Contracts.Identity.Requests;
using Skimart.Contracts.Identity.Responses;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Identity.Mappers;

public static class IdentityMappers
{
    public static LoginCommand ToCommand(this LoginRequest loginRequest)
    {
        return new LoginCommand(loginRequest.Email, loginRequest.Password);
    }
    
    public static RegisterCommand ToCommand(this RegisterRequest registerRequest)
    {
        return new RegisterCommand(
            registerRequest.DisplayName, 
            registerRequest.Email, 
            registerRequest.Password);
    }
    
    public static UpdateAddressCommand ToCommand(this AddressUpdateRequest addressUpdateRequest)
    {
        return new UpdateAddressCommand(
            addressUpdateRequest.FirstName,
            addressUpdateRequest.LastName,
            addressUpdateRequest.Street,
            addressUpdateRequest.City,
            addressUpdateRequest.Province,
            addressUpdateRequest.ZipCode);
    }
    
    public static UserResponse ToResponse(this UserDto userDto)
    {
        return new UserResponse(userDto.Email, userDto.DisplayName, userDto.Token);
    }
    
    public static CurrentUserResponse ToResponse(this CurrentUserDto currentUserDto)
    {
        return new CurrentUserResponse(currentUserDto.Email, currentUserDto.DisplayName);
    }

    public static AddressResponse ToResponse(this Address address)
    {
        return new AddressResponse(
            address.FirstName,
            address.LastName,
            address.Street,
            address.City,
            address.Province,
            address.ZipCode);
    }
}