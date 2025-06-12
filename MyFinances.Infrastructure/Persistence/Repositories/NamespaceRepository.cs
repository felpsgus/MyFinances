using Microsoft.EntityFrameworkCore;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Repositories;
using MyFinances.Infrastructure.Persistence.Context;

namespace MyFinances.Infrastructure.Persistence.Repositories;

public class NamespaceRepository : INamespaceRepository
{
    private readonly MyFinancesDbContext _dbContext;
    private readonly IUserContext _userContext;

    public NamespaceRepository(MyFinancesDbContext dbContext, IUserContext userContext)
    {
        _dbContext = dbContext;
        _userContext = userContext;
    }

    public async Task<Namespace> AddAsync(Namespace entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Namespaces.AddAsync(entity, cancellationToken);
        return entity;
    }

    private IQueryable<Namespace> Query()
    {
        return _dbContext.Namespaces
            .Where(n => n.UserId == _userContext.UserId ||
                        n.Family.FamilyMembers.Any(fm => fm.UserId == _userContext.UserId));
    }

    public async Task<IEnumerable<Namespace>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Query().AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Namespace?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(n => n.Expenses).ThenInclude(e => e.ExpenseTags).ThenInclude(et => et.Tag)
            .Include(n => n.Debts)
            .AsTracking()
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
    }

    public Namespace Delete(Namespace @namespace)
    {
        _dbContext.Namespaces.Remove(@namespace);
        return @namespace;
    }

    public async Task<IReadOnlyCollection<Tag>> GetTagsAsync(Guid namespaceId,
        CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(n => n.Tags)
            .Where(n => n.Id == namespaceId)
            .SelectMany(n => n.Tags)
            .ToListAsync(cancellationToken);
    }
}