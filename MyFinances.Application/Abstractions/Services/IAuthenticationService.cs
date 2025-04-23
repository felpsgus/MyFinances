using MyFinances.Domain.Entities;

namespace MyFinances.Application.Abstractions.Services;

public interface IAuthenticationService
{
    (string token, DateTime expiration) GenerateJwtToken(User user);

    (string refreshToken, DateTime expiration) GenerateRefreshToken();

    Guid GetUserIdFromExpiredToken(string token);
}