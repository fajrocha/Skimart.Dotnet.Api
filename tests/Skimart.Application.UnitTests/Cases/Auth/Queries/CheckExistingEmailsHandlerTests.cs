using Moq;
using Skimart.Application.Abstractions.Auth;
using Skimart.Application.Cases.Auth.Queries.CheckExistingEmail;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Application.UnitTests.Cases.Auth.Queries;

public class CheckExistingEmailsHandlerTests
{
    private readonly Mock<IAuthService> _authServiceMock;

    public CheckExistingEmailsHandlerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
    }

    [Fact]
    public async Task Handle_WhenUserExists_ReturnsTrue()
    {
        var query = new CheckExistingEmailQuery("test@email.com");

        var existingUser = new AppUser
        {
            DisplayName = "name",
        };

        _authServiceMock.Setup(asm => asm.FindUserByEmailAsync(query.Email)).ReturnsAsync(existingUser);

        var handler = new CheckExistingEmailHandler(_authServiceMock.Object);

        var result = await handler.Handle(query, It.IsAny<CancellationToken>());
        
        Assert.True(result);
    }
    
    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ReturnsFalse()
    {
        var query = new CheckExistingEmailQuery("test@email.com");

        AppUser? existingUser = null;

        _authServiceMock.Setup(asm => asm.FindUserByEmailAsync(query.Email)).ReturnsAsync(existingUser);

        var handler = new CheckExistingEmailHandler(_authServiceMock.Object);

        var result = await handler.Handle(query, It.IsAny<CancellationToken>());
        
        Assert.False(result);
    }
    
    [Fact]
    public async Task Handle_WhenFindUserByEmailThrowsException_RethrowsException()
    {
        var query = new CheckExistingEmailQuery("test@email.com");

        _authServiceMock.Setup(asm => asm.FindUserByEmailAsync(query.Email)).Throws<Exception>();

        var handler = new CheckExistingEmailHandler(_authServiceMock.Object);

        await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(query, It.IsAny<CancellationToken>()));
    }
}