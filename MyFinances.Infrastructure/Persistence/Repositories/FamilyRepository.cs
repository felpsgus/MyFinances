using Microsoft.EntityFrameworkCore;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Repositories;
using MyFinances.Infrastructure.Persistence.Context;

namespace MyFinances.Infrastructure.Persistence.Repositories;

public class FamilyRepository : IFamilyRepository
{
    private readonly MyFinancesDbContext _myFinancesDbContext;
    private readonly IUserContext _userContext;

    public FamilyRepository(MyFinancesDbContext myFinancesDbContext, IUserContext userContext)
    {
        _myFinancesDbContext = myFinancesDbContext;
        _userContext = userContext;
    }

    public async Task<Family> AddAsync(Family family, CancellationToken cancellationToken = default)
    {
        await _myFinancesDbContext.Families.AddAsync(family, cancellationToken);
        return family;
    }

    private IQueryable<Family> GetQueryable()
    {
        return _myFinancesDbContext.Families
            .Include(f => f.FamilyMembers).ThenInclude(fm => fm.User)
            .Where(f => f.FamilyMembers.Any(fm => fm.UserId == _userContext.UserId));
    }

    public async Task<Family?> GetByIdAsync(Guid familyId, CancellationToken cancellationToken = default)
    {
        return await GetQueryable().FirstOrDefaultAsync(f => f.Id == familyId, cancellationToken);
    }

    public async Task<List<Family>> GetAllFamiliesAsync(CancellationToken cancellationToken = default)
    {
        return await GetQueryable().ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetQueryable().AnyAsync(f => f.Id == id, cancellationToken);
    }
}