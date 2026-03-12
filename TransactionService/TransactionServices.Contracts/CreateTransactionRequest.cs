namespace TransactionServices.Contracts
{
    public sealed class CreateTransactionRequest
    {
        public Guid UserId { get; init; }
        public decimal Amount { get; init; }
        public string Description { get; init; }
    }
}
