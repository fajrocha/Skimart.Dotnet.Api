using FluentResults;
using MapsterMapper;
using MediatR;
using Skimart.Application.Abstractions.Auth;
using Skimart.Application.Cases.Auth.Dtos;
using Skimart.Application.Cases.Auth.Errors;

namespace Skimart.Application.Cases.Auth.Queries.GetUserAddress;

public class GetUserAddressHandler : IRequestHandler<GetUserAddressQuery, Result<AddressDto>>
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public GetUserAddressHandler(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }
    
    public async Task<Result<AddressDto>> Handle(GetUserAddressQuery query, CancellationToken cancellationToken)
    {
        var user = await _authService.FindAddressByEmailFromClaims(query.Claims);

        if (user is null)
        {
            return Result.Fail(AppIdentityError.AddressNotFound);
        }

        var addressDto = _mapper.Map<AddressDto>(user.Address);
        
        return Result.Ok(addressDto);
    }
}