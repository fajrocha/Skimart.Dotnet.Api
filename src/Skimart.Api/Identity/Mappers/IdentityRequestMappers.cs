using Skimart.Application.Identity.Commands.Login;
using Skimart.Application.Identity.Commands.Register;
using Skimart.Application.Identity.Commands.UpdateAddress;
using Skimart.Contracts.Identity.Requests;

namespace Skimart.Identity.Mappers;

public static class IdentityRequestMappers
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
}