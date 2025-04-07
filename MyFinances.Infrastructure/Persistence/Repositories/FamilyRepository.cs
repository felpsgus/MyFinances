using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyFinances.Application.Abstractions.Repositories;
using MyFinances.Application.Families.Views;
using MyFinances.Domain.Entities;
using MyFinances.Infrastructure.Persistence.Context;

namespace MyFinances.Infrastructure.Persistence.Repositories;

public class FamilyRepository : IFamilyRepository
{
    private readonly MyFinancesDbContext _myFinancesDbContext;
    private readonly IMapper _mapper;

    public FamilyRepository(MyFinancesDbContext myFinancesDbContext, IMapper mapper)
    {
        _myFinancesDbContext = myFinancesDbContext;
        _mapper = mapper;
    }

    public async Task AddAsync(Family family, CancellationToken cancellationToken = default)
    {
        await _myFinancesDbContext.Families.AddAsync(family, cancellationToken);
    }

    public async Task<FamilyViewModel?> GetByIdAsync(Guid familyId, CancellationToken cancellationToken = default)
    {
        return await _myFinancesDbContext.Families
            .AsNoTracking()
            .Include(f => f.FamilyMembers)
            .ThenInclude(fm => fm.User)
            .ProjectTo<FamilyViewModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(f => f.Id == familyId, cancellationToken);
    }
}