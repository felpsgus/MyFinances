using MyFinances.Domain.Enum;

namespace MyFinances.Application.Abstractions.Interfaces;

public interface IUserContext
{
    Guid UserId { get; }
    string Email { get; }
    bool IsAdmin { get; }
    Role Role { get; }
}