namespace MyFinances.Infrastructure.Authentication;

public class JwtOptions
{
    public string Audience { get; set; }

    public string Issuer { get; set; }

    public string SecretKey { get; set; }

    public int ExpirationInMinutes { get; set; }

    public int RefreshTokenExpirationInDays { get; set; }
}