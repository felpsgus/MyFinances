using MyFinances.Application.Abstractions.Interfaces;

namespace MyFinances.Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    string Name,
    string Email,
    DateOnly BirthDate,
    string Password)
    : ICommand<Guid>;