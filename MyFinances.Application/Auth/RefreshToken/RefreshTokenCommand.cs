using MediatR;
using MyFinances.Application.Auth.Views;

namespace MyFinances.Application.Auth.RefreshToken;

public record RefreshTokenCommand(string Token, string RefreshToken) : IRequest<LoginViewModel>;