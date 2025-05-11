using MyFinances.Domain.Entities;

namespace MyFinances.Domain.Repositories;

public interface INamespaceRepository
{
    Task<Namespace> AddAsync(Namespace entity, CancellationToken cancellationToken = default);

    Task<List<Namespace>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Namespace?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Namespace Delete(Namespace @namespace, CancellationToken cancellationToken = default);
}