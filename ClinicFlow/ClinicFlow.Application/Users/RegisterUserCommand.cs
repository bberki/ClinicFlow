using MediatR;

namespace ClinicFlow.Application.Users;

public record RegisterUserCommand(string Username, string Password) : IRequest;
