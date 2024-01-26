using System.Security.Claims;
using AutoFixture;
using DeepEqual.Syntax;
using MapsterMapper;
using Moq;
using Skimart.Application.Abstractions.Auth;
using Skimart.Application.Cases.Auth.Dtos;
using Skimart.Application.Cases.Auth.Errors;
using Skimart.Application.Cases.Auth.Queries.GetCurrentLoggedUser;
using Skimart.Application.Extensions.FluentResults;
using Skimart.Application.Mappers;
using Skimart.Application.UnitTests.Cases.Auth.Shared;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Application.UnitTests.Cases.Auth.Queries;

public class GetCurrentLoggedUserHandlerTests
{
    private readonly Mock<IAuthService> _authServiceMock = new();
    private readonly ClaimsPrincipal _claimsPrincipal = AuthShared.GetValidClaimsPrincipal();
    private readonly Fixture _fixture = new();
    private readonly Mock<ITokenService> _tokenService = new();

    [Fact]
    public async Task Handle_WhenUserIsNotFound_ReturnsFailureAndNoUser()
    {
        var query = new GetCurrentLoggedUserQuery(_claimsPrincipal);
        AppUser? user = null;

        _authServiceMock.Setup(asm => asm.FindByEmailFromClaims(_claimsPrincipal)).ReturnsAsync(user);

        var handler = new GetCurrentLoggedUserHandler(_authServiceMock.Object, _tokenService.Object);
        var result = await handler.Handle(query, It.IsAny<CancellationToken>());
        
        Assert.True(result.IsFailed);
        Assert.Contains(AppIdentityError.NoLoggedUser.Message, result.GetReasonsAsCollection());
    }
    
    [Fact]
    public async Task Handle_WhenUserIsFound_ReturnsSuccessAndUser()
    {
        var query = new GetCurrentLoggedUserQuery(_claimsPrincipal);
        var user = new AppUser
        {
            DisplayName = "Name",
            Email = "test@email.com"
        };
        var token = _fixture.Create<string>();
        _authServiceMock.Setup(asm => asm.FindByEmailFromClaims(_claimsPrincipal)).ReturnsAsync(user);
        _tokenService.Setup(ts => ts.CreateToken(user)).Returns(token);
        var expectedUserDto = new UserDto(user.Email, user.DisplayName, token);
        
        var handler = new GetCurrentLoggedUserHandler(_authServiceMock.Object, _tokenService.Object);
        var result = await handler.Handle(query, It.IsAny<CancellationToken>());
        var actualUserDto = result.Value;
        
        Assert.True(result.IsSuccess);
        expectedUserDto.ShouldDeepEqual(actualUserDto);
    }
    
    [Fact]
    public async Task Handle_WhenFindByEmailThrowsException_RethrowsException()
    {
        var query = new GetCurrentLoggedUserQuery(_claimsPrincipal);
        _authServiceMock.Setup(asm => asm.FindByEmailFromClaims(_claimsPrincipal)).Throws<Exception>();
        
        var handler = new GetCurrentLoggedUserHandler(_authServiceMock.Object, _tokenService.Object);

        await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(query, It.IsAny<CancellationToken>()));
    }
    
    [Fact]
    public async Task Handle_WhenTokenGenerationThrowsException_RethrowsException()
    {
        var query = new GetCurrentLoggedUserQuery(_claimsPrincipal);
        var user = new AppUser
        {
            DisplayName = "Name",
            Email = "test@email.com"
        };
        _authServiceMock.Setup(asm => asm.FindByEmailFromClaims(_claimsPrincipal)).ReturnsAsync(user);
        _tokenService.Setup(ts => ts.CreateToken(user)).Throws<Exception>();
        
        var handler = new GetCurrentLoggedUserHandler(_authServiceMock.Object, _tokenService.Object);
        
        await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(query, It.IsAny<CancellationToken>()));
    }
}