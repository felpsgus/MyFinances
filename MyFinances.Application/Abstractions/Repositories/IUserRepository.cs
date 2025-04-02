using MyFinances.Domain.Entities;

namespace MyFinances.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);

    void Delete(User user);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<User?> CheckEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<User?> GetUserByCredentials(string email, string password, CancellationToken cancellationToken = default);
}