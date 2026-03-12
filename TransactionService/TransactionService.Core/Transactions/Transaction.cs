namespace TransactionService.Core.Transactions
{
    public sealed class Transaction
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public decimal Amount { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Transaction() { }

        public Transaction(Guid id, Guid userId, decimal amount, string description)
        {
            Id = id;
            UserId = userId;
            Amount = amount;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
