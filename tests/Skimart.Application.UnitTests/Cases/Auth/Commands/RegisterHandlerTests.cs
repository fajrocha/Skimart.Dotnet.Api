using AutoFixture;
using DeepEqual.Syntax;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using Skimart.Application.Abstractions.Auth;
using Skimart.Application.Cases.Auth.Commands.Register;
using Skimart.Application.Cases.Auth.Dtos;
using Skimart.Application.Cases.Auth.Errors;
using Skimart.Application.Extensions.FluentResults;
using Skimart.Application.Mappers;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Application.UnitTests.Cases.Auth.Commands;

public class RegisterHandlerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Fixture _fixture;
    private readonly IMapper _mapper;

    public RegisterHandlerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _fixture = new Fixture();
        _mapper = AppMapper.GetMapper();
    }

    [Fact]
    public async Task Handle_UserAlreadyExists_ReturnsFailure()
    {
        var command = new RegisterCommand("TestUser", "user@email.com", "Pwd12345!");
        
        var existingUser = new AppUser
        {
            DisplayName = "TestUser"
        };

        _authServiceMock.Setup(asm => asm.FindUserByEmailAsync(command.Email)).ReturnsAsync(existingUser);

        var handler = new RegisterHandler(_authServiceMock.Object, _tokenServiceMock.Object, _mapper);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());
        
        Assert.True(result.IsFailed);
        Assert.Contains(AppIdentityError.UserAlreadyExists.Message, result.GetReasonsAsCollection());
    }
    
    [Fact]
    public async Task Handle_UserCreationFails_ReturnsFailure()
    {
        var command = new RegisterCommand("TestUser", "user@email.com", "Pwd12345!");

        AppUser? existingUser = null;

        _authServiceMock.Setup(asm => asm.FindUserByEmailAsync(command.Email)).ReturnsAsync(existingUser);
        _authServiceMock.Setup(asm => asm.CreateUserAsync(It.IsAny<AppUser>(), command.Password)).ReturnsAsync(false);

        var handler = new RegisterHandler(_authServiceMock.Object, _tokenServiceMock.Object, _mapper);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());
        
        Assert.True(result.IsFailed);
        Assert.Contains(AppIdentityError.UserRegistrationFailed.Message, result.GetReasonsAsCollection());
    }
    
    [Fact]
    public async Task Handle_UserCreationSucceeds_ReturnsSuccess()
    {
        var command = new RegisterCommand("TestUser", "user@email.com", "Pwd12345!");
        var token = _fixture.Create<string>();

        AppUser? existingUser = null;

        _authServiceMock.Setup(asm => asm.FindUserByEmailAsync(command.Email)).ReturnsAsync(existingUser);
        _authServiceMock.Setup(asm => asm.CreateUserAsync(It.IsAny<AppUser>(), command.Password)).ReturnsAsync(true);
        _tokenServiceMock.Setup(ts => ts.CreateToken(It.IsAny<AppUser>())).Returns(token);

        var expectedUserDto = new UserDto(command.Email, command.DisplayName, token);

        var handler = new RegisterHandler(_authServiceMock.Object, _tokenServiceMock.Object, _mapper);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());
        var actualUserDto = result.Value;
        
        Assert.True(result.IsSuccess);
        expectedUserDto.ShouldDeepEqual(actualUserDto);
    }
    
    [Fact]
    public async Task Handle_UserExistenceCheckThrowsException_RethrowsException()
    {
        var command = new RegisterCommand("TestUser", "user@email.com", "Pwd12345!");

        _authServiceMock.Setup(asm => asm.FindUserByEmailAsync(command.Email)).Throws<Exception>();

        var handler = new RegisterHandler(_authServiceMock.Object, _tokenServiceMock.Object, _mapper);

        await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, It.IsAny<CancellationToken>()));
    }
    
    [Fact]
    public async Task Handle_UserCreationThrowsException_RethrowsException()
    {
        var command = new RegisterCommand("TestUser", "user@email.com", "Pwd12345!");

        AppUser? existingUser = null;

        _authServiceMock.Setup(asm => asm.FindUserByEmailAsync(command.Email)).ReturnsAsync(existingUser);
        _authServiceMock.Setup(asm => asm.CreateUserAsync(It.IsAny<AppUser>(), command.Password)).Throws<Exception>();

        var handler = new RegisterHandler(_authServiceMock.Object, _tokenServiceMock.Object, _mapper);

        await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, It.IsAny<CancellationToken>()));
    }
    
    [Fact]
    public async Task Handle_TokenGenerationThrowsException_RethrowsException()
    {
        var command = new RegisterCommand("TestUser", "user@email.com", "Pwd12345!");
        var token = _fixture.Create<string>();

        AppUser? existingUser = null;

        _authServiceMock.Setup(asm => asm.FindUserByEmailAsync(command.Email)).ReturnsAsync(existingUser);
        _authServiceMock.Setup(asm => asm.CreateUserAsync(It.IsAny<AppUser>(), command.Password)).ReturnsAsync(true);
        _tokenServiceMock.Setup(ts => ts.CreateToken(It.IsAny<AppUser>())).Throws<Exception>();

        var handler = new RegisterHandler(_authServiceMock.Object, _tokenServiceMock.Object, _mapper);

        await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, It.IsAny<CancellationToken>()));
    }
}