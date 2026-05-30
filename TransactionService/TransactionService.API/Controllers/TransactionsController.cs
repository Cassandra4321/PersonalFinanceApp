using BuildingBlocks.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Contracts;
using TransactionService.Core.Abstractions;
using TransactionService.Core.Transactions;

namespace TransactionService.API.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public sealed class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;

        public TransactionsController(
            ITransactionRepository repository,
            IPublishEndpoint publishEndpoint
        )
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

            await _publishEndpoint.Publish(
                new TransactionCreatedEvent
                {
                    TransactionId = transaction.Id,
                    UserId = transaction.UserId,
                    Amount = transaction.Amount,
                    Description = transaction.Description,
                    CreatedAt = transaction.CreatedAt,
                },
                cancellationToken
            );

            var response = new TransactionResponse
            {
                Id = transaction.Id,
                UserId = transaction.UserId,
                Amount = transaction.Amount,
                Description = transaction.Description,
                CreatedAt = transaction.CreatedAt,
            };

            return CreatedAtAction(
                nameof(Create),
                new { id = transaction.Id },
                MapToResponse(transaction)
            );
        }

        private static TransactionResponse MapToResponse(Transaction t) =>
            new()
            {
                Id = t.Id,
                UserId = t.UserId,
                Amount = t.Amount,
                Description = t.Description,
                CreatedAt = t.CreatedAt,
            };
    }
}
