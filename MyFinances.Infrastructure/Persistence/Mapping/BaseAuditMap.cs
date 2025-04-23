using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinances.Domain.Primitives;

namespace MyFinances.Infrastructure.Persistence.Mapping;

public abstract class BaseAuditMap<TEntity> : BaseMap<TEntity> where TEntity : AuditEntity
{
    protected override bool MapFieldsOnBaseMap => false;

    public new void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(p => p.CreatedBy)
            .WithMany()
            .HasForeignKey(p => p.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(p => p.UpdatedBy)
            .WithMany()
            .HasForeignKey(p => p.UpdatedById)
            .OnDelete(DeleteBehavior.Restrict);

        MapFields(builder);
    }
}