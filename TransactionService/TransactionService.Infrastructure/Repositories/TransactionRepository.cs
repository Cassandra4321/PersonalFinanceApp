using Microsoft.EntityFrameworkCore;
using TransactionService.Core.Abstractions;
using TransactionService.Core.Transactions;
using TransactionService.Infrastructure.Clients;
using TransactionService.Infrastructure.Persistence;

namespace TransactionService.Infrastructure.Repositories
{
    public sealed class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionDbContext _dbContext;
        private readonly UserServiceClient _userServiceClient;

        public TransactionRepository(
            TransactionDbContext dbContext,
            UserServiceClient userServiceClient
        )
        {
            _dbContext = dbContext;
            _userServiceClient = userServiceClient;
        }

        public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken)
        {
            var userExists = await _userServiceClient.UserExistsAsync(
                transaction.UserId,
                cancellationToken
            );

            if (!userExists)
            {
                throw new Exception("User does not exist");
            }

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
