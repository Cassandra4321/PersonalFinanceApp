using Microsoft.AspNetCore.Mvc;
using TransactionService.Contracts;
using TransactionService.Core.Abstractions;
using TransactionService.Core.Transactions;

namespace TransactionService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _repository;

        public TransactionsController(ITransactionRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<TransactionResponse>> Create(
            CreateTransactionRequest request,
            CancellationToken cancellationToken
        )
        {
            var transaction = new Transaction(
                Guid.NewGuid(),
                request.UserId,
                request.Amount,
                request.Description
            );

            await _repository.AddAsync(transaction, cancellationToken);

            var response = new TransactionResponse
            {
                Id = transaction.Id,
                UserId = transaction.UserId,
                Amount = transaction.Amount,
                Description = transaction.Description,
                CreatedAt = transaction.CreatedAt,
            };

            return Ok(response);
        }
    }
}
