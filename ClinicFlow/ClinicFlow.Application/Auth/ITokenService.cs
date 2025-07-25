using ClinicFlow.Domain.Entities;

namespace ClinicFlow.Application.Auth;

public interface ITokenService
{
    string GenerateToken(User user);
}
