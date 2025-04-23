using MyFinances.Domain.Entities;

namespace MyFinances.Domain.Repositories;

public interface IUserRepository
{
    Task<User> AddAsync(User entity, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}