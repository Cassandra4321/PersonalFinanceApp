namespace BudgetService.Core.Budgets;

public sealed class Budget
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public decimal MonthlyLimit { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Budget() { }

    public Budget(Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        MonthlyLimit = 0;
        CreatedAt = DateTime.UtcNow;
    }

    public void SetMonthlyLimit(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Budget cannot be negative");

        MonthlyLimit = amount;
    }
}
