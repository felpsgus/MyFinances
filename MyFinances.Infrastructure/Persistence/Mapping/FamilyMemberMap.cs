using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinances.Domain.Entities;

namespace MyFinances.Infrastructure.Persistence.Mapping;

public class FamilyMemberMap : BaseMap<FamilyMember>
{
    protected override string TableName => nameof(FamilyMember);

    protected override void MapFields(EntityTypeBuilder<FamilyMember> builder)
    {
        builder
            .HasOne(fm => fm.User)
            .WithMany()
            .HasForeignKey(fm => fm.UserId);

        builder
            .HasOne(fm => fm.Family)
            .WithMany(f => f.FamilyMembers)
            .HasForeignKey(fm => fm.FamilyId);
    }
}