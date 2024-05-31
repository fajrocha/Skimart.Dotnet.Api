using Skimart.Application.Identity.DTOs;
using Skimart.Contracts.Identity.Responses;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Identity.Mappers;

public static class IdentityResponseMappers
{
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