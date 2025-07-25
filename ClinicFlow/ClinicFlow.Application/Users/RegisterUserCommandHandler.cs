using MediatR;

namespace ClinicFlow.Application.Users;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
{
    public Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Demo implementation - always succeeds
        return Task.FromResult(true);
    }
}
