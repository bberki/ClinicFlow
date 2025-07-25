using MediatR;

namespace ClinicFlow.Application.Users;

public record LoginQuery(string Username, string Password) : IRequest<bool>;
