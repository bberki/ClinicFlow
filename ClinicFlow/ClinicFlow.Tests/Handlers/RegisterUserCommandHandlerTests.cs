using ClinicFlow.Application.Users;
using ClinicFlow.Infrastructure.Data;
using ClinicFlow.Infrastructure.Repositories;
using ClinicFlow.Application.Common;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClinicFlow.Tests.Handlers;

public class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_AddsUserToRepository()
    {
        var options = new DbContextOptionsBuilder<ClinicFlowDbContext>()
            .UseInMemoryDatabase("register_test")
            .Options;
        using var context = new ClinicFlowDbContext(options);
        IUserRepository repo = new UserRepository(context);
        var handler = new RegisterUserCommandHandler(repo);

        var command = new RegisterUserCommand("testuser", "secret");
        await handler.Handle(command, CancellationToken.None);

        Assert.Equal(1, context.Users.Count());
        Assert.NotEqual("secret", context.Users.First().PasswordHash);
    }
}
