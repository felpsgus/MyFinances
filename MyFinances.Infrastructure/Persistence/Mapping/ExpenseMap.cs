using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinances.Domain.Entities;

namespace MyFinances.Infrastructure.Persistence.Mapping;

public class ExpenseMap : BaseAuditMap<Expense>
{
    protected override string TableName => nameof(Expense);

    protected override void MapFields(EntityTypeBuilder<Expense> builder)
    {
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.Value)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(e => e.Paid)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.PaymentDate)
            .HasColumnType("date");

        builder.HasOne(e => e.ResponsiblePerson)
            .WithMany()
            .HasForeignKey(e => e.ResponsiblePersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ComplexProperty(e => e.Period);

        builder.HasOne(e => e.Debt)
            .WithMany()
            .HasForeignKey(e => e.DebtId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.InstallmentNumber)
            .IsRequired(false);

        builder.HasMany(e => e.Tags)
            .WithMany();
    }
}