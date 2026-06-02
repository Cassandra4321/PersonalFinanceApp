using BudgetService.Core.Budgets;

namespace BudgetService.UnitTests.Domain;

public sealed class BudgetTests
{
    [Fact]
    public void Constructor_ValidUserId_CreatesBudget()
    {
        var userId = Guid.NewGuid();

        var budget = new Budget(userId);

        Assert.Equal(userId, budget.UserId);
        Assert.Equal(0m, budget.MonthlyLimit);
        Assert.Equal(0m, budget.TotalSpent);
        Assert.NotEqual(Guid.Empty, budget.Id);
    }

    [Fact]
    public void Constructor_ValidUserId_SetsCreatedAtToUtcNow()
    {
        var before = DateTime.UtcNow;

        var budget = new Budget(Guid.NewGuid());

        var after = DateTime.UtcNow;
        Assert.InRange(budget.CreatedAt, before, after);
    }

    [Fact]
    public void SetMonthlyLimit_ValidAmount_SetsLimit()
    {
        var budget = new Budget(Guid.NewGuid());

        budget.SetMonthlyLimit(5000m);

        Assert.Equal(5000m, budget.MonthlyLimit);
    }

    [Fact]
    public void SetMonthlyLimit_NegativeAmount_ThrowsArgumentException()
    {
        var budget = new Budget(Guid.NewGuid());

        Assert.Throws<ArgumentException>(() => budget.SetMonthlyLimit(-1m));
    }

    [Fact]
    public void SetMonthlyLimit_ZeroAmount_SetsLimit()
    {
        var budget = new Budget(Guid.NewGuid());

        budget.SetMonthlyLimit(0m);

        Assert.Equal(0m, budget.MonthlyLimit);
    }

    [Fact]
    public void AddTransaction_ValidAmount_UpdatesTotalSpent()
    {
        var budget = new Budget(Guid.NewGuid());

        budget.AddTransaction(150m);

        Assert.Equal(150m, budget.TotalSpent);
    }

    [Fact]
    public void AddTransaction_MultipleTransactions_AccumulatesTotalSpent()
    {
        var budget = new Budget(Guid.NewGuid());

        budget.AddTransaction(150m);
        budget.AddTransaction(200m);
        budget.AddTransaction(50m);

        Assert.Equal(400m, budget.TotalSpent);
    }

    [Fact]
    public void AddTransaction_NegativeAmount_DecreasesTotalSpent()
    {
        var budget = new Budget(Guid.NewGuid());
        budget.AddTransaction(200m);

        budget.AddTransaction(-50m);

        Assert.Equal(150m, budget.TotalSpent);
    }
}
