using ClinicFlow.Application.Users;
using ClinicFlow.Infrastructure.Data;
using ClinicFlow.Infrastructure.Repositories;
using ClinicFlow.Infrastructure.Services;
using ClinicFlow.Application.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Xunit;

namespace ClinicFlow.Tests.Handlers;

public class LoginQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsToken_WhenCredentialsValid()
    {
        var options = new DbContextOptionsBuilder<ClinicFlowDbContext>()
            .UseInMemoryDatabase("login_test")
            .Options;
        using var context = new ClinicFlowDbContext(options);
        IUserRepository repo = new UserRepository(context);
        var registerHandler = new RegisterUserCommandHandler(repo);
        await registerHandler.Handle(new RegisterUserCommand("user", "pass"), CancellationToken.None);

        var settings = Options.Create(new JwtSettings { Key = "TestKey1234567890", Issuer = "Test", Audience = "Test" });
        ITokenService tokenService = new TokenService(settings);
        var handler = new LoginQueryHandler(repo, tokenService);

        var token = await handler.Handle(new LoginQuery("user", "pass"), CancellationToken.None);

        Assert.False(string.IsNullOrEmpty(token));
    }
}
