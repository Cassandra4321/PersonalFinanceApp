using BuildingBlocks.Contracts;
using MassTransit;

namespace TransactionService.API.Consumers
{
    public sealed class UserCreatedConsumer : IConsumer<UserCreatedEvent>
    {
        public Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var message = context.Message;

            Console.WriteLine($"User created event received for: {message.Email}");

            return Task.CompletedTask;
        }
    }
}
