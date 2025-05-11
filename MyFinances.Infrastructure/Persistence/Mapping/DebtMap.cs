using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinances.Domain.Entities;

namespace MyFinances.Infrastructure.Persistence.Mapping;

public class DebtMap : BaseAuditMap<Debt>
{
    protected override string TableName => nameof(Debt);

    protected override void MapFields(EntityTypeBuilder<Debt> builder)
    {
        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Description)
            .HasMaxLength(500);

        builder.Property(d => d.Value)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(d => d.Paid)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(d => d.PaymentDate)
            .HasColumnType("date");

        builder.ComplexProperty(d => d.StartDate)
            .IsRequired();

        builder.Property(d => d.DueDay)
            .IsRequired();

        builder.Property(d => d.Installments)
            .IsRequired();

        builder.Property(d => d.ValuePaid)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasMany(d => d.InstallmentsList)
            .WithOne()
            .HasForeignKey(i => i.DebtId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}