using BuildingBlocks.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TransactionService.API.Controllers;
using TransactionService.Contracts;
using TransactionService.Core.Abstractions;
using TransactionService.Core.Transactions;

namespace TransactionService.UnitTests.Controllers;

public sealed class TransactionsControllerTests
{
    private readonly Mock<ITransactionRepository> _mockRepository;
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
    private readonly TransactionsController _controller;

    public TransactionsControllerTests()
    {
        _mockRepository = new Mock<ITransactionRepository>();
        _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        _controller = new TransactionsController(
            _mockRepository.Object,
            _mockPublishEndpoint.Object
        );
    }

    [Fact]
    public async Task Create_ValidRequest_Returns201Created()
    {
        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var request = new CreateTransactionRequest
        {
            UserId = Guid.NewGuid(),
            Amount = 150.00m,
            Description = "Food shop",
        };

        var result = await _controller.Create(request, CancellationToken.None);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var response = Assert.IsType<TransactionResponse>(createdResult.Value);
        Assert.Equal(request.UserId, response.UserId);
        Assert.Equal(request.Amount, response.Amount);
        Assert.Equal(request.Description, response.Description);
    }

    [Fact]
    public async Task Create_ValidRequest_PublishesTransactionCreatedEvent()
    {
        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var request = new CreateTransactionRequest
        {
            UserId = Guid.NewGuid(),
            Amount = 150.00m,
            Description = "Food shop",
        };

        await _controller.Create(request, CancellationToken.None);

        _mockPublishEndpoint.Verify(
            p => p.Publish(It.IsAny<TransactionCreatedEvent>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task Create_ValidRequest_CallsRepositoryOnce()
    {
        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var request = new CreateTransactionRequest
        {
            UserId = Guid.NewGuid(),
            Amount = 150.00m,
            Description = "Food shop",
        };

        await _controller.Create(request, CancellationToken.None);

        _mockRepository.Verify(
            r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task Create_RepositoryThrowsException_ExceptionPropagates()
    {
        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("User does not exist"));

        var request = new CreateTransactionRequest
        {
            UserId = Guid.NewGuid(),
            Amount = 150.00m,
            Description = "Food shop",
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _controller.Create(request, CancellationToken.None)
        );
    }
}
