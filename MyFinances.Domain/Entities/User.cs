using MyFinances.Domain.Enum;
using MyFinances.Domain.Shared;

namespace MyFinances.Domain.Entities;

public class User : Entity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public string Password { get; private set; }
    public RoleEnum Role { get; private set; } = RoleEnum.User;
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiration { get; private set; }

    public User(string name, string email, DateOnly birthDate, string password)
    {
        Name = name;
        Email = email;
        BirthDate = birthDate;
        Password = password;
    }

    public void UpdateToken(string refreshToken, DateTime? expiration = null)
    {
        RefreshToken = refreshToken;
        if (expiration != null)
            RefreshTokenExpiration = expiration;
    }
}