using System.Threading;
using System.Threading.Tasks;
using ClinicFlow.Application.Greetings;
using Xunit;

public class GetGreetingQueryTests
{
    [Fact]
    public async Task Handler_Returns_Greeting()
    {
        var handler = new GetGreetingQueryHandler();
        var result = await handler.Handle(new GetGreetingQuery(), CancellationToken.None);
        Assert.Equal("Hello from CQRS", result);
    }
}
