using BudgetService.Core.Budgets;

namespace BudgetService.Core.Abstractions
{
    public interface IBudgetRepository
    {
        Task AddAsync(Budget budget, CancellationToken cancellationToken);
        Task<Budget?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task UpdateAsync(Budget budget, CancellationToken cancellationToken);
    }
}
