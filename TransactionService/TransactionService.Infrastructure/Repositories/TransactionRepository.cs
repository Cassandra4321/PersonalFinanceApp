using Microsoft.EntityFrameworkCore;
using TransactionService.Core.Abstractions;
using TransactionService.Core.Transactions;
using TransactionService.Infrastructure.Persistence;

namespace TransactionService.Infrastructure.Repositories
{
    public sealed class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionDbContext _dbContext;

        public TransactionRepository(TransactionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken)
        {
            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Transaction>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken
        )
        {
            return await _dbContext
                .Transactions.Where(t => t.UserId == userId)
                .ToListAsync(cancellationToken);
        }
    }
}
