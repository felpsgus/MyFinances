using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinances.Domain.Entities;

namespace MyFinances.Infrastructure.Persistence.Mapping;

public class NamespaceMap : BaseAuditMap<Namespace>
{
    protected override string TableName => nameof(Namespace);

    protected override void MapFields(EntityTypeBuilder<Namespace> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Type)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired(false);

        builder.Property(x => x.FamilyId)
            .IsRequired(false);

        builder.HasMany(x => x.Tags)
            .WithOne()
            .HasForeignKey(x => x.NamespaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Expenses)
            .WithOne()
            .HasForeignKey(x => x.NamespaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Debts)
            .WithOne()
            .HasForeignKey(x => x.NamespaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}