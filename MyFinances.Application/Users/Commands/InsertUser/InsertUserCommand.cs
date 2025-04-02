using MyFinances.Application.Abstractions;
using MyFinances.Application.Abstractions.Interfaces;

namespace MyFinances.Application.Users.Commands.InsertUser;

public record InsertUserCommand(
    string Name,
    string Email,
    DateOnly BirthDate,
    string Password)
    : ICommand<Guid>;