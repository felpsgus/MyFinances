using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinances.Domain.Entities;

namespace MyFinances.Infrastructure.Persistence.Mapping;

public class ExpenseTagMap : IEntityTypeConfiguration<ExpenseTag>
{
    public void Configure(EntityTypeBuilder<ExpenseTag> builder)
    {
        builder
            .ToTable(nameof(ExpenseTag));

        builder
            .HasKey(e => new { e.TagId, e.ExpenseId });

        builder
            .HasOne(e => e.Tag)
            .WithMany()
            .HasForeignKey(e => e.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Expense>()
            .WithMany()
            .HasForeignKey(e => e.ExpenseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}