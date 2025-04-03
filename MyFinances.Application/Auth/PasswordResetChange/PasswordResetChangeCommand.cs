using MyFinances.Application.Abstractions.Interfaces;

namespace MyFinances.Application.Auth.PasswordResetChange;

public record PasswordResetChangeCommand(
    string Email,
    string Token,
    string Password,
    string ConfirmPassword) : ICommand;