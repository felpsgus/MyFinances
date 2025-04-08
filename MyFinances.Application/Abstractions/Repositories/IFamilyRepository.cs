using MyFinances.Application.Families.Views;
using MyFinances.Domain.Entities;

namespace MyFinances.Application.Abstractions.Repositories;

public interface IFamilyRepository : IBaseRepository<Family>
{
    Task<List<FamilyViewModel>> GetAllFamiliesAsync(CancellationToken cancellationToken = default);
}