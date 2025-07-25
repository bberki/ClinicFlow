using MediatR;

namespace ClinicFlow.Application.Users;

public class LoginQueryHandler : IRequestHandler<LoginQuery, bool>
{
    public Task<bool> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var valid = !string.IsNullOrWhiteSpace(request.Username) &&
                    !string.IsNullOrWhiteSpace(request.Password);
        return Task.FromResult(valid);
    }
}
