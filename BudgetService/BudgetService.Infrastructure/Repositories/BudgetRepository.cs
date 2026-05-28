using BudgetService.Core.Abstractions;
using BudgetService.Core.Budgets;
using BudgetService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BudgetService.Infrastructure.Repositories
{
    public sealed class BudgetRepository : IBudgetRepository
    {
        private readonly BudgetDbContext _dbContext;

        public BudgetRepository(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Budget budget, CancellationToken cancellationToken)
        {
            _dbContext.Budgets.Add(budget);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Budget?> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken
        )
        {
            return await _dbContext.Budgets.FirstOrDefaultAsync(
                b => b.UserId == userId,
                cancellationToken
            );
        }

        public async Task UpdateAsync(Budget budget, CancellationToken cancellationToken)
        {
            _dbContext.Budgets.Update(budget);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
