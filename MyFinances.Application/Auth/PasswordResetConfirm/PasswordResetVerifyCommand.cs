using MediatR;

namespace MyFinances.Application.Auth.PasswordResetConfirm;

public record PasswordResetVerifyCommand(string Email, string Token) : IRequest;