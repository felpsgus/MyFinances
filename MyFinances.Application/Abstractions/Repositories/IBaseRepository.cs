using MyFinances.Domain.Shared;

namespace MyFinances.Application.Abstractions.Repositories;

public interface IBaseRepository<TEntity> where TEntity : Entity
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    void Delete(TEntity entity);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}