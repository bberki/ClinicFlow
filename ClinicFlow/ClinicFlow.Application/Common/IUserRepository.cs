namespace ClinicFlow.Application.Common;

using ClinicFlow.Domain.Entities;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
}
