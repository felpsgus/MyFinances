using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinances.Domain.Entities;

namespace MyFinances.Infrastructure.Persistence.Mapping;

public class InstallmentMap : BaseAuditMap<Installment>
{
    protected override string TableName => nameof(Installment);

    protected override void MapFields(EntityTypeBuilder<Installment> builder)
    {
        builder.Property(i => i.Number)
            .IsRequired();

        builder.Property(i => i.Value)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(i => i.DueDate)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(i => i.Paid)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(i => i.PaymentDate)
            .HasColumnType("date");
    }
}