using TransactionService.Core.Transactions;

namespace TransactionService.Core.Abstractions;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken);

    Task<IReadOnlyList<Transaction>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken
    );
}
