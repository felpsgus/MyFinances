using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinances.Domain.Entities;

namespace MyFinances.Infrastructure.Persistence.Mapping;

public class TagMap : BaseAuditMap<Tag>
{
    protected override string TableName => nameof(Tag);

    protected override void MapFields(EntityTypeBuilder<Tag> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}