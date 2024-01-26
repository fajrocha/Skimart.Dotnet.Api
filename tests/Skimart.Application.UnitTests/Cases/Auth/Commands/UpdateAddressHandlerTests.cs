using System.Security.Claims;
using AutoFixture;
using DeepEqual.Syntax;
using MapsterMapper;
using Moq;
using Skimart.Application.Abstractions.Auth;
using Skimart.Application.Cases.Auth.Commands.UpdateAddress;
using Skimart.Application.Cases.Auth.Dtos;
using Skimart.Application.Cases.Auth.Errors;
using Skimart.Application.Extensions.FluentResults;
using Skimart.Application.Mappers;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Application.UnitTests.Cases.Auth.Commands;

public class UpdateAddressHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly ClaimsPrincipal _claimsPrincipal;

    public UpdateAddressHandlerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _mapper = AppMapper.GetMapper();
        _fixture = new Fixture();
        
        var claims = new List<Claim>() 
        { 
            new Claim(ClaimTypes.Email, "test@test.com"),
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        _claimsPrincipal = new ClaimsPrincipal(identity);
    }


    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ReturnsFailureAndDoesNotUpdate()
    {
        var address = _fixture.Create<AddressDto>();
        var command = new UpdateAddressCommand(address, _claimsPrincipal);
        AppUser? nonExistentUser = null;

        _authServiceMock.Setup(asm => asm.FindAddressByEmailFromClaims(command.Claims)).ReturnsAsync(nonExistentUser);

        var handler = new UpdateAddressHandler(_authServiceMock.Object, _mapper);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());
        
        Assert.True(result.IsFailed);
        Assert.Contains(AppIdentityError.UserNotFound.Message, result.GetReasonsAsCollection());
    }
    
    [Fact]
    public async Task Handle_WhenUpdateFails_ReturnsFailureAndDoesNotUpdate()
    {
        var address = _fixture.Create<AddressDto>();
        var command = new UpdateAddressCommand(address, _claimsPrincipal);
        var existingUser = new AppUser
        {
            DisplayName = "TestUser"
        };;

        _authServiceMock.Setup(asm => asm.FindAddressByEmailFromClaims(command.Claims)).ReturnsAsync(existingUser);
        _authServiceMock.Setup(asm => asm.UpdateAddressAsync(existingUser, It.IsAny<Address>())).ReturnsAsync(false);

        var handler = new UpdateAddressHandler(_authServiceMock.Object, _mapper);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());
        
        Assert.True(result.IsFailed);
        Assert.Contains(AppIdentityError.AddressUpdateFailed.Message, result.GetReasonsAsCollection());
    }
    
    [Fact]
    public async Task Handle_WhenUpdateSucceeds_ReturnsFailureAndDoesNotUpdate()
    {
        var expectedAddress = _fixture.Create<AddressDto>();
        var command = new UpdateAddressCommand(expectedAddress, _claimsPrincipal);
        var existingUser = new AppUser
        {
            DisplayName = "TestUser"
        };;

        _authServiceMock.Setup(asm => asm.FindAddressByEmailFromClaims(command.Claims)).ReturnsAsync(existingUser);
        _authServiceMock.Setup(asm => asm.UpdateAddressAsync(existingUser, It.IsAny<Address>())).ReturnsAsync(true);

        var handler = new UpdateAddressHandler(_authServiceMock.Object, _mapper);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());
        var actualAddress = result.Value;
        
        Assert.True(result.IsSuccess);
        expectedAddress.ShouldDeepEqual(actualAddress);
    }
    
    [Fact]
    public async Task Handle_WhenFindingUserThrowsException_RethrowsException()
    {
        var expectedAddress = _fixture.Create<AddressDto>();
        var command = new UpdateAddressCommand(expectedAddress, _claimsPrincipal);

        _authServiceMock.Setup(asm => asm.FindAddressByEmailFromClaims(command.Claims)).Throws<Exception>();

        var handler = new UpdateAddressHandler(_authServiceMock.Object, _mapper);

        await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, It.IsAny<CancellationToken>()));
    }
    
    [Fact]
    public async Task Handle_WhenUpdatingAddressThrowsException_RethrowsException()
    {
        var expectedAddress = _fixture.Create<AddressDto>();
        var command = new UpdateAddressCommand(expectedAddress, _claimsPrincipal);
        var existingUser = new AppUser
        {
            DisplayName = "TestUser"
        };;

        _authServiceMock.Setup(asm => asm.FindAddressByEmailFromClaims(command.Claims)).ReturnsAsync(existingUser);
        _authServiceMock.Setup(asm => asm.UpdateAddressAsync(existingUser, It.IsAny<Address>())).Throws<Exception>();

        var handler = new UpdateAddressHandler(_authServiceMock.Object, _mapper);

        await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, It.IsAny<CancellationToken>()));
    }
}