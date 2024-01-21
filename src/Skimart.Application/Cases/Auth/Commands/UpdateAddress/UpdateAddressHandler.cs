using System.Diagnostics.CodeAnalysis;
using FluentResults;
using MapsterMapper;
using MediatR;
using Skimart.Application.Abstractions.Auth;
using Skimart.Application.Cases.Auth.Dtos;
using Skimart.Application.Cases.Auth.Errors;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Application.Cases.Auth.Commands.UpdateAddress;

public class UpdateAddressHandler : IRequestHandler<UpdateAddressCommand, Result<AddressDto>>
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public UpdateAddressHandler(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }
    
    public async Task<Result<AddressDto>> Handle(UpdateAddressCommand command, CancellationToken cancellationToken)
    {
        var user = await _authService.FindAddressByEmailFromClaims(command.Claims);

        if (user is null)
        {
            return Result.Fail(AppIdentityError.UserNotFound);
        }

        var addressDto = command.AddressDto;
        var address = _mapper.Map<Address>(addressDto);

        var result = await _authService.UpdateAddressAsync(user, address);
        
        return result ? Result.Ok(addressDto) : Result.Fail(AppIdentityError.AddressUpdateFailed);
    }
}