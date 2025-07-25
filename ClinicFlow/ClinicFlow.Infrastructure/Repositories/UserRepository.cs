using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClinicFlow.Application.Auth;
using ClinicFlow.Domain.Entities;

namespace ClinicFlow.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new()
    {
        new User { Id = 1, Username = "admin", Password = "password", Role = "Admin" },
        new User { Id = 2, Username = "user", Password = "password", Role = "User" }
    };

    public Task<User?> GetByCredentialsAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var user = _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        return Task.FromResult(user);
    }
}
