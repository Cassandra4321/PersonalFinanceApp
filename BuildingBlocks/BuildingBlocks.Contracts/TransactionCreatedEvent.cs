namespace BuildingBlocks.Contracts;

public sealed class TransactionCreatedEvent
{
    public Guid TransactionId { get; init; }
    public Guid UserId { get; init; }
    public decimal Amount { get; init; }
    public string Description { get; init; } = default!;
    public DateTime CreatedAt { get; init; }
}
