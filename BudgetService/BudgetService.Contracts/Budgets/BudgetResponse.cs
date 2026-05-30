namespace BudgetService.Contracts.Budgets
{
    public sealed class BudgetResponse
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public decimal MonthlyLimit { get; init; }
        public decimal TotalSpent { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
