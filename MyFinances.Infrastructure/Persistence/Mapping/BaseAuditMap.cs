using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinances.Domain.Shared;

namespace MyFinances.Infrastructure.Persistence.Mapping;

public abstract class BaseAuditMap<TEntity> : BaseMap<TEntity> where TEntity : AuditEntity
{
    protected override bool MapFieldsOnBaseMap => false;

    public new void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(p => p.CreatedByUser)
            .WithMany()
            .HasForeignKey(p => p.CreatedBy)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(p => p.UpdatedByUser)
            .WithMany()
            .HasForeignKey(p => p.UpdatedBy)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        MapFields(builder);
    }
}