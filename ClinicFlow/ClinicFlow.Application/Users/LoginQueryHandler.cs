using System.Security.Cryptography;
using System.Text;
using ClinicFlow.Application.Common;
using MediatR;

namespace ClinicFlow.Application.Users;

public class LoginQueryHandler : IRequestHandler<LoginQuery, string>
{
    private readonly IUserRepository _repository;
    private readonly ITokenService _tokenService;

    public LoginQueryHandler(IUserRepository repository, ITokenService tokenService)
    {
        _repository = repository;
        _tokenService = tokenService;
    }

    public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByUsernameAsync(request.Username, cancellationToken);
        if (user is null)
        {
            return string.Empty;
        }

        var hash = HashPassword(request.Password);
        if (user.PasswordHash != hash)
        {
            return string.Empty;
        }

        return _tokenService.GenerateToken(user);
    }

    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
