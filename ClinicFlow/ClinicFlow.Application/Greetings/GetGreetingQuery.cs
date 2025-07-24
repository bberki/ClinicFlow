using MediatR;

namespace ClinicFlow.Application.Greetings;

public record GetGreetingQuery() : IRequest<string>;
