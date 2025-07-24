using MediatR;

namespace ClinicFlow.Application.Greetings;

public record GetGreetingQuery : IRequest<string>;

public class GetGreetingQueryHandler : IRequestHandler<GetGreetingQuery, string>
{
    public Task<string> Handle(GetGreetingQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult("Hello from CQRS");
    }
}
