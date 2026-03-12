using Microsoft.EntityFrameworkCore;
using TransactionService.Core.Transactions;

namespace TransactionService.Infrastructure.Persistence
{
    public sealed class TransactionDbContext : DbContext
    {
        public TransactionDbContext(DbContextOptions<TransactionDbContext> options)
            : base(options) { }

        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransactionDbContext).Assembly);
        }
    }
}
