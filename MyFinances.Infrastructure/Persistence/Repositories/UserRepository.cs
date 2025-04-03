using Microsoft.EntityFrameworkCore;
using MyFinances.Application.Abstractions.Repositories;
using MyFinances.Domain.Entities;
using MyFinances.Infrastructure.Persistence.Context;

namespace MyFinances.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MyFinancesDbContext _myFinancesDbContext;

    public UserRepository(MyFinancesDbContext myFinancesDbContext)
    {
        _myFinancesDbContext = myFinancesDbContext;
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _myFinancesDbContext.Users.AddAsync(user, cancellationToken);
    }

    public void Delete(User user)
    {
        _myFinancesDbContext.Users.Remove(user);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _myFinancesDbContext.Users.AnyAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _myFinancesDbContext.Users
            .IgnoreAutoIncludes()
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _myFinancesDbContext.Users.SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}