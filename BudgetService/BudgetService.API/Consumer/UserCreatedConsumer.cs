using BudgetService.Core.Budgets;
using BudgetService.Infrastructure.Persistence;
using BuildingBlocks.Contracts;
using MassTransit;

namespace BudgetService.API.Consumers;

public sealed class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly BudgetDbContext _db;

    public UserCreatedConsumer(BudgetDbContext db)
    {
        _db = db;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var message = context.Message;

        var budget = new Budget(message.Id);

        await _db.Budgets.AddAsync(budget);
        await _db.SaveChangesAsync();
    }
}
