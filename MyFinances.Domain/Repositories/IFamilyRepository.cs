using MyFinances.Domain.Entities;

namespace MyFinances.Domain.Repositories;

public interface IFamilyRepository
{
    Task<Family> AddAsync(Family entity, CancellationToken cancellationToken = default);

    Task<Family?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Family>> GetAllFamiliesAsync(CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}