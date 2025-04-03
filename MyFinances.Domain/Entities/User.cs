using System.Security.Cryptography;
using System.Text;
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

    private User()
    {
    }

    public static User Create(string name, string email, DateOnly birthDate, string password)
    {
        var user = new User
        {
            Name = name,
            Email = email,
            BirthDate = birthDate,
            Password = ComputeHash(password)
        };
        return user;
    }

    public void UpdateToken(string refreshToken, DateTime? expiration = null)
    {
        RefreshToken = refreshToken;
        if (expiration != null)
            RefreshTokenExpiration = expiration;
    }

    public void UpdatePassword(string password)
    {
        Password = ComputeHash(password);
    }

    public bool ValidatePassword(string password)
    {
        var hashedPassword = ComputeHash(password);
        return Password.Equals(hashedPassword, StringComparison.OrdinalIgnoreCase);
    }

    private static string ComputeHash(string password)
    {
        var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

        var builder = new StringBuilder();
        foreach (var t in hashedBytes) builder.Append(t.ToString("X"));

        return hash;
    }
}