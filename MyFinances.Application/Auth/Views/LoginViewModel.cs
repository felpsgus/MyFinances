namespace MyFinances.Application.Auth.Views;

public record LoginViewModel(string Token, string RefreshToken, DateTime Expiration);