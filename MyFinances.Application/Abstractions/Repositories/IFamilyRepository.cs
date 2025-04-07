using MyFinances.Application.Families.Views;
using MyFinances.Domain.Entities;

namespace MyFinances.Application.Abstractions.Repositories;

public interface IFamilyRepository
{
    Task AddAsync(Family family, CancellationToken cancellationToken = default);

    Task<FamilyViewModel?> GetByIdAsync(Guid familyId, CancellationToken cancellationToken = default);
}