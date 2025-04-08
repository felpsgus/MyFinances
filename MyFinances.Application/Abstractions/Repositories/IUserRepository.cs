using MyFinances.Domain.Entities;

namespace MyFinances.Application.Abstractions.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}