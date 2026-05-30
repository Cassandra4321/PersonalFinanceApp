using BudgetService.Core.Abstractions;
using BudgetService.Core.Budgets;
using BuildingBlocks.Contracts;
using MassTransit;

namespace BudgetService.API.Consumers;

public sealed class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly IBudgetRepository _budgetRepository;

    public UserCreatedConsumer(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var message = context.Message;
        var budget = new Budget(message.Id);
        await _budgetRepository.AddAsync(budget, context.CancellationToken);
    }
}
