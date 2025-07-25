using ClinicFlow.Domain.Entities;

namespace ClinicFlow.Application.Auth;

public interface IUserRepository
{
    Task<User?> GetByCredentialsAsync(string username, string password, CancellationToken cancellationToken = default);
}
