using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Domain.Primitives;

namespace MyFinances.Infrastructure.Persistence.Interceptors;

public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly IUserContext _userContext;

    public AuditInterceptor(IUserContext userContext)
    {
        _userContext = userContext;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = eventData.Context.ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Modified or EntityState.Added);

        foreach (var entry in entries)
        {
            if (entry.Entity is not AuditEntity entity) continue;

            if (entry.State == EntityState.Added)
                entity.CreateEntity(_userContext.UserId);

            entity.UpdateEntity(_userContext.UserId);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}