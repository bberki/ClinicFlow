using System.Threading;
using System.Threading.Tasks;
using ClinicFlow.Application.Greetings;
using Xunit;

namespace ClinicFlow.Tests.Handlers;

public class GreetingHandlerTests
{
    [Fact]
    public async Task GetGreeting_ReturnsExpectedMessage()
    {
        var handler = new GetGreetingQueryHandler();
        var result = await handler.Handle(new GetGreetingQuery(), CancellationToken.None);
        Assert.Equal("Hello from MediatR!", result);
    }
}
