using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Enum;

namespace MyFinances.Infrastructure.Persistence.Mapping;

public class UserMap : BaseMap<User>
{
    protected override string TableName => nameof(User);

    protected override void MapFields(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(u => u.Role)
            .HasConversion(
                to => to.ToString(),
                from => Enum.Parse<Role>(from))
            .IsRequired();
    }
}