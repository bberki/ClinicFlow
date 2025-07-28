using System.Security.Cryptography;
using System.Text;
using ClinicFlow.Application.Common;
using ClinicFlow.Domain.Entities;
using MediatR;

namespace ClinicFlow.Application.Users;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly IUserRepository _repository;

    public RegisterUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var hash = HashPassword(request.Password);
        var user = new User { Username = request.Username, PasswordHash = hash };
        await _repository.AddAsync(user, cancellationToken);
        return Unit.Value;
    }

    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
