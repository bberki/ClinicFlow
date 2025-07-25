namespace ClinicFlow.Application.Common;

using ClinicFlow.Domain.Entities;

public interface ITokenService
{
    string GenerateToken(User user);
}
