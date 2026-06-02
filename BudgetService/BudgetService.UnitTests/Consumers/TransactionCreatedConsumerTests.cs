using BudgetService.API.Consumers;
using BudgetService.Core.Abstractions;
using BudgetService.Core.Budgets;
using BuildingBlocks.Contracts;
using MassTransit;
using Moq;

namespace BudgetService.UnitTests.Consumers;

public sealed class TransactionCreatedConsumerTests
{
    private readonly Mock<IBudgetRepository> _mockRepository;
    private readonly TransactionCreatedConsumer _consumer;

    public TransactionCreatedConsumerTests()
    {
        _mockRepository = new Mock<IBudgetRepository>();
        _consumer = new TransactionCreatedConsumer(_mockRepository.Object);
    }

    [Fact]
    public async Task Consume_ValidEvent_UpdatesBudgetTotalSpent()
    {
        var userId = Guid.NewGuid();
        var budget = new Budget(userId);

        _mockRepository
            .Setup(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(budget);

        var message = new TransactionCreatedEvent
        {
            TransactionId = Guid.NewGuid(),
            UserId = userId,
            Amount = 150m,
            Description = "Food shop",
            CreatedAt = DateTime.UtcNow,
        };

        var context = new Mock<ConsumeContext<TransactionCreatedEvent>>();
        context.Setup(c => c.Message).Returns(message);
        context.Setup(c => c.CancellationToken).Returns(CancellationToken.None);

        await _consumer.Consume(context.Object);

        Assert.Equal(150m, budget.TotalSpent);
        _mockRepository.Verify(
            r => r.UpdateAsync(It.IsAny<Budget>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task Consume_BudgetNotFound_DoesNotCallUpdate()
    {
        _mockRepository
            .Setup(r => r.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Budget?)null);

        var message = new TransactionCreatedEvent
        {
            TransactionId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Amount = 150m,
            Description = "Food shop",
            CreatedAt = DateTime.UtcNow,
        };

        var context = new Mock<ConsumeContext<TransactionCreatedEvent>>();
        context.Setup(c => c.Message).Returns(message);
        context.Setup(c => c.CancellationToken).Returns(CancellationToken.None);

        await _consumer.Consume(context.Object);

        _mockRepository.Verify(
            r => r.UpdateAsync(It.IsAny<Budget>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }
}
