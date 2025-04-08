using Microsoft.EntityFrameworkCore;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Abstractions.Repositories;
using MyFinances.Application.Families.Views;
using MyFinances.Domain.Entities;
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

    public async Task AddAsync(Family family, CancellationToken cancellationToken = default)
    {
        await _myFinancesDbContext.Families.AddAsync(family, cancellationToken);
    }

    public void Delete(Family family)
    {
        _myFinancesDbContext.Families.Remove(family);
    }

    public async Task<bool> ExistsAsync(Guid familyId, CancellationToken cancellation)
    {
        return await _myFinancesDbContext.Families.AnyAsync(f => f.Id == familyId, cancellation);
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

    public async Task<List<FamilyViewModel>> GetAllFamiliesAsync(CancellationToken cancellationToken = default)
    {
        return await GetQueryable().Select(f => FamilyViewModel.FromEntity(f, _userContext))
            .ToListAsync(cancellationToken);
    }
}