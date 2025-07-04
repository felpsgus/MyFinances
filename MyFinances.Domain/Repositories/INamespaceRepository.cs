using MyFinances.Domain.Entities;

namespace MyFinances.Domain.Repositories;

public interface INamespaceRepository
{
    Task<Namespace> AddAsync(Namespace entity, CancellationToken cancellationToken = default);

    Task<IEnumerable<Namespace>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Namespace?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Namespace Delete(Namespace @namespace);

    Task<IReadOnlyCollection<Tag>> GetTagsAsync(Guid namespaceId, CancellationToken cancellationToken = default);
}