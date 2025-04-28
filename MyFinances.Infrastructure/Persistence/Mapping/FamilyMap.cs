using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinances.Domain.Entities;

namespace MyFinances.Infrastructure.Persistence.Mapping;

public class FamilyMap : BaseAuditMap<Family>
{
    protected override string TableName => nameof(Family);

    protected override void MapFields(EntityTypeBuilder<Family> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}