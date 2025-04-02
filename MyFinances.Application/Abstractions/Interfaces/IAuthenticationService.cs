using MyFinances.Domain.Entities;

namespace MyFinances.Application.Abstractions.Interfaces;

public interface IAuthenticationService
{
    string ComputeHash(string password);

    (string token, DateTime expiration) GenerateJwtToken(User user);

    (string refreshToken, DateTime expiration) GenerateRefreshToken();

    Guid GetUserIdFromExpiredToken(string token);
}