using MediatR;
using MyFinances.Application.Auth.Views;

namespace MyFinances.Application.Auth.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginViewModel>;