using BudgetService.API.Consumers;
using BudgetService.Core.Abstractions;
using BudgetService.Core.Budgets;
using BuildingBlocks.Contracts;
using MassTransit;
using Moq;

namespace BudgetService.UnitTests.Consumers;

public sealed class UserCreatedConsumerTests
{
    private readonly Mock<IBudgetRepository> _mockRepository;
    private readonly UserCreatedConsumer _consumer;

    public UserCreatedConsumerTests()
    {
        _mockRepository = new Mock<IBudgetRepository>();
        _consumer = new UserCreatedConsumer(_mockRepository.Object);
    }

    [Fact]
    public async Task Consume_ValidEvent_CreatesBudget()
    {
        var userId = Guid.NewGuid();
        var message = new UserCreatedEvent
        {
            Id = userId,
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "Testsson",
        };

        var context = new Mock<ConsumeContext<UserCreatedEvent>>();
        context.Setup(c => c.Message).Returns(message);
        context.Setup(c => c.CancellationToken).Returns(CancellationToken.None);

        await _consumer.Consume(context.Object);

        _mockRepository.Verify(
            r => r.AddAsync(It.Is<Budget>(b => b.UserId == userId), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task Consume_ValidEvent_CallsRepositoryOnce()
    {
        var message = new UserCreatedEvent
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "Testsson",
        };

        var context = new Mock<ConsumeContext<UserCreatedEvent>>();
        context.Setup(c => c.Message).Returns(message);
        context.Setup(c => c.CancellationToken).Returns(CancellationToken.None);

        await _consumer.Consume(context.Object);

        _mockRepository.Verify(
            r => r.AddAsync(It.IsAny<Budget>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
