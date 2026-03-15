namespace TransactionService.Contracts
{
    public sealed class TransactionResponse
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public decimal Amount { get; init; }
        public string Description { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }
}
