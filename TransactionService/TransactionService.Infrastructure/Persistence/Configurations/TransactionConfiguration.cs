using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Core.Transactions;

namespace TransactionService.Infrastructure.Persistence.Configurations
{
    public sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.UserId).IsRequired();

            builder.Property(t => t.Amount).HasColumnType("decimal(18,2)").IsRequired();

            builder.Property(t => t.Description).HasMaxLength(500);

            builder.Property(t => t.CreatedAt).IsRequired();
        }
    }
}
