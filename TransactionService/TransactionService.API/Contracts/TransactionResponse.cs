namespace TransactionService.API.Contracts
{
    public sealed class TransactionResponse
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public decimal Amount { get; init; }
        public string Description { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
