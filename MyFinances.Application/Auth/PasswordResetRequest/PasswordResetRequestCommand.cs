using MediatR;

namespace MyFinances.Application.Auth.PasswordResetRequest;

public record PasswordResetRequestCommand(string Email) : IRequest;