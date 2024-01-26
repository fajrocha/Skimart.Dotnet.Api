using System.Security.Claims;
using AutoFixture;
using DeepEqual.Syntax;
using MapsterMapper;
using Moq;
using Skimart.Application.Abstractions.Auth;
using Skimart.Application.Cases.Auth.Dtos;
using Skimart.Application.Cases.Auth.Errors;
using Skimart.Application.Cases.Auth.Queries.GetUserAddress;
using Skimart.Application.Extensions.FluentResults;
using Skimart.Application.Mappers;
using Skimart.Application.UnitTests.Cases.Auth.Shared;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Application.UnitTests.Cases.Auth.Queries;

public class GetUserAddressHandlerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly ClaimsPrincipal _claimsPrincipal;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public GetUserAddressHandlerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _mapper = AppMapper.GetMapper();
        _claimsPrincipal = AuthShared.GetValidClaimsPrincipal();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ReturnsFailureAndNoAddress()
    {
        var query = new GetUserAddressQuery(_claimsPrincipal);

        AppUser? user = null;
        
        _authServiceMock.Setup(asm => asm.FindAddressByEmailFromClaims(query.Claims)).ReturnsAsync(user);

        var handler = new GetUserAddressHandler(_authServiceMock.Object, _mapper);

        var result = await handler.Handle(query, It.IsAny<CancellationToken>());
        
        Assert.True(result.IsFailed);
        Assert.Contains(AppIdentityError.AddressNotFound.Message, result.GetReasonsAsCollection());
    }
    
    [Fact]
    public async Task Handle_WhenUserExists_ReturnsSuccessAndAddress()
    {
        var query = new GetUserAddressQuery(_claimsPrincipal);
        var address = new Address
        {
            FirstName = "Test",
            LastName = "McTest",
            Street = "Test St. N35",
            City = "Teston",
            Province = "Testylvania",
            ZipCode = "1000"
        };
        var user = new AppUser
        {
            DisplayName = "Name",
            Address = address
        };
        var expectedAddressDto = _mapper.Map<AddressDto>(address);
        _authServiceMock.Setup(asm => asm.FindAddressByEmailFromClaims(query.Claims)).ReturnsAsync(user);
        
        var handler = new GetUserAddressHandler(_authServiceMock.Object, _mapper);
        
        var result = await handler.Handle(query, It.IsAny<CancellationToken>());
        var actualAddressDto = result.Value;
        
        Assert.True(result.IsSuccess);
        expectedAddressDto.ShouldDeepEqual(actualAddressDto);
    }
    
    [Fact]
    public async Task Handle_WhenFindAddressThrowsException_RethrowsException()
    {
        var query = new GetUserAddressQuery(_claimsPrincipal);
        
        _authServiceMock.Setup(asm => asm.FindAddressByEmailFromClaims(query.Claims)).Throws<Exception>();
        
        var handler = new GetUserAddressHandler(_authServiceMock.Object, _mapper);
        
        await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(query, It.IsAny<CancellationToken>()));
    }
}