using AutoFixture;
using DeepEqual.Syntax;
using Moq;
using Skimart.Application.Abstractions.Auth;
using Skimart.Application.Cases.Auth.Commands.Login;
using Skimart.Application.Cases.Auth.Dtos;
using Skimart.Application.Cases.Auth.Errors;
using Skimart.Application.Extensions.FluentResults;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Application.UnitTests.Cases.Auth.Commands;

public class LoginHandlerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Fixture _fixture;

    public LoginHandlerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ReturnsFailure()
    {
        var loginCommand = _fixture.Create<LoginCommand>();

        AppUser? nonExistentUser = null;
        _authServiceMock.Setup(asm => asm.FindUserByEmailAsync(loginCommand.Email)).ReturnsAsync(nonExistentUser);

        var handler = new LoginHandler(_authServiceMock.Object, _tokenServiceMock.Object);

        var result = await handler.Handle(loginCommand, It.IsAny<CancellationToken>());
        
        Assert.True(result.IsFailed);
        Assert.Contains(AppIdentityError.UserLoginFailed.Message, result.GetReasonsAsCollection());
    }
    
    [Fact]
    public async Task Handle_WhenPasswordIsWrong_ReturnsFailure()
    {
        var loginCommand = _fixture.Create<LoginCommand>();
        var user = new AppUser
        {
            DisplayName = "TestUser"
        };

        _authServiceMock.Setup(asm => asm.FindUserByEmailAsync(loginCommand.Email)).ReturnsAsync(user);
        _authServiceMock.Setup(asm => asm.CheckPasswordAsync(user ,loginCommand.Password)).ReturnsAsync(false);

        var handler = new LoginHandler(_authServiceMock.Object, _tokenServiceMock.Object);

        var result = await handler.Handle(loginCommand, It.IsAny<CancellationToken>());
        
        Assert.True(result.IsFailed);
        Assert.Contains(AppIdentityError.UserLoginFailed.Message, result.GetReasonsAsCollection());
    }
    
    [Fact]
    public async Task Handle_WhenUserExistsAndPasswordIsRight_ReturnsSuccessAndToken()
    {
        var loginCommand = _fixture.Create<LoginCommand>();
        var user = new AppUser
        {
            DisplayName = "TestUser",
            Email = "email@test.com"
        };
        var token = _fixture.Create<string>();
        var expectedUserDto = new UserDto(user.Email, user.DisplayName, token);

        _authServiceMock.Setup(asm => asm.FindUserByEmailAsync(loginCommand.Email)).ReturnsAsync(user);
        _authServiceMock.Setup(asm => asm.CheckPasswordAsync(user ,loginCommand.Password)).ReturnsAsync(true);
        _tokenServiceMock.Setup(tg => tg.CreateToken(user)).Returns(token);

        var handler = new LoginHandler(_authServiceMock.Object, _tokenServiceMock.Object);

        var result = await handler.Handle(loginCommand, It.IsAny<CancellationToken>());
        var actualUserDto = result.Value;
        
        Assert.True(result.IsSuccess);
        expectedUserDto.ShouldDeepEqual(actualUserDto);
    }
    
    [Fact]
    public void Handle_WhenUserCheckingThrowsException_RethrowsException()
    {
        var loginCommand = _fixture.Create<LoginCommand>();
        var user = new AppUser
        {
            DisplayName = "TestUser",
            Email = "email@test.com"
        };

        _authServiceMock.Setup(asm => asm.FindUserByEmailAsync(loginCommand.Email)).Throws<Exception>();

        var handler = new LoginHandler(_authServiceMock.Object, _tokenServiceMock.Object);

        Assert.ThrowsAsync<Exception>(async () => await handler.Handle(loginCommand, It.IsAny<CancellationToken>()));
    }
    
    [Fact]
    public void Handle_WhenUserCheckingPasswordThrowsExceptions_RethrowsException()
    {
        var loginCommand = _fixture.Create<LoginCommand>();
        var user = new AppUser
        {
            DisplayName = "TestUser",
            Email = "email@test.com"
        };

        _authServiceMock.Setup(asm => asm.FindUserByEmailAsync(loginCommand.Email)).ReturnsAsync(user);
        _authServiceMock.Setup(asm => asm.CheckPasswordAsync(user ,loginCommand.Password)).Throws<Exception>();

        var handler = new LoginHandler(_authServiceMock.Object, _tokenServiceMock.Object);

        Assert.ThrowsAsync<Exception>(async () => await handler.Handle(loginCommand, It.IsAny<CancellationToken>()));
    }
    
    [Fact]
    public void Handle_WhenTokenGenerationThrowsException_RethrowsException()
    {
        var loginCommand = _fixture.Create<LoginCommand>();
        var user = new AppUser
        {
            DisplayName = "TestUser",
            Email = "email@test.com"
        };

        _authServiceMock.Setup(asm => asm.FindUserByEmailAsync(loginCommand.Email)).ReturnsAsync(user);
        _authServiceMock.Setup(asm => asm.CheckPasswordAsync(user ,loginCommand.Password)).ReturnsAsync(true);
        _tokenServiceMock.Setup(tg => tg.CreateToken(user)).Throws<Exception>();

        var handler = new LoginHandler(_authServiceMock.Object, _tokenServiceMock.Object);

        Assert.ThrowsAsync<Exception>(async () => await handler.Handle(loginCommand, It.IsAny<CancellationToken>()));
    }
}