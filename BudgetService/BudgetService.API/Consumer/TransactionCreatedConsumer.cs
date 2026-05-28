using BudgetService.Core.Abstractions;
using BuildingBlocks.Contracts;
using MassTransit;

namespace BudgetService.API.Consumers;

public sealed class TransactionCreatedConsumer : IConsumer<TransactionCreatedEvent>
{
    private readonly IBudgetRepository _budgetRepository;

    public TransactionCreatedConsumer(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task Consume(ConsumeContext<TransactionCreatedEvent> context)
    {
        var message = context.Message;

        var budget = await _budgetRepository.GetByUserIdAsync(
            message.UserId,
            context.CancellationToken
        );

        if (budget is null)
        {
            Console.WriteLine($"No budget found for UserId: {message.UserId}");
            return;
        }

        budget.AddTransaction(message.Amount);
        await _budgetRepository.UpdateAsync(budget, context.CancellationToken);
    }
}
