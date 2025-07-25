using ClinicFlow.Application.Common;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ClinicFlowDbContext _context;

    public UserRepository(ClinicFlowDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }
}
