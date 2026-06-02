using TransactionService.Core.Transactions;

namespace TransactionService.UnitTests.Domain;

public sealed class TransactionTests
{
    [Fact]
    public void Constructor_ValidData_CreatesTransaction()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var transaction = new Transaction(id, userId, 150.00m, "Food shop");

        Assert.Equal(id, transaction.Id);
        Assert.Equal(userId, transaction.UserId);
        Assert.Equal(150.00m, transaction.Amount);
        Assert.Equal("Food shop", transaction.Description);
    }

    [Fact]
    public void Constructor_ValidData_SetsCreatedAtToUtcNow()
    {
        var before = DateTime.UtcNow;

        var transaction = new Transaction(Guid.NewGuid(), Guid.NewGuid(), 150.00m, "Test");

        var after = DateTime.UtcNow;
        Assert.InRange(transaction.CreatedAt, before, after);
    }

    [Fact]
    public void Constructor_ZeroAmount_CreatesTransaction()
    {
        var transaction = new Transaction(Guid.NewGuid(), Guid.NewGuid(), 0m, "Test");

        Assert.Equal(0m, transaction.Amount);
    }

    [Fact]
    public void Constructor_NegativeAmount_CreatesTransaction()
    {
        var transaction = new Transaction(Guid.NewGuid(), Guid.NewGuid(), -50.00m, "Refund");

        Assert.Equal(-50.00m, transaction.Amount);
    }

    [Fact]
    public void Constructor_EmptyDescription_CreatesTransaction()
    {
        var transaction = new Transaction(Guid.NewGuid(), Guid.NewGuid(), 150.00m, "");

        Assert.Equal("", transaction.Description);
    }
}
