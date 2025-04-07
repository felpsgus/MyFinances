using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinances.Domain.Entities;

namespace MyFinances.Infrastructure.Persistence.Mapping;

public class FamilyMap : BaseAuditMap<Family>
{
    protected override string TableName => nameof(Family);

    protected override void MapFields(EntityTypeBuilder<Family> builder)
    {
        builder
            .HasMany<User>()
            .WithMany()
            .UsingEntity<FamilyMember>(
                j => j
                    .HasOne(fm => fm.User)
                    .WithMany()
                    .HasForeignKey(fm => fm.UserId),
                j => j
                    .HasOne(fm => fm.Family)
                    .WithMany(f => f.FamilyMembers)
                    .HasForeignKey(fm => fm.FamilyId),
                j =>
                {
                    j.ToTable(nameof(FamilyMember));
                });
    }
}