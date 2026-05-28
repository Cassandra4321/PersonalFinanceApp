using BudgetService.Core.Budgets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetService.Infrastructure.Persistence.Configurations;

public sealed class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.ToTable("Budgets");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.UserId).IsRequired();
        builder.Property(b => b.MonthlyLimit).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(b => b.TotalSpent).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(b => b.CreatedAt).IsRequired();
    }
}
