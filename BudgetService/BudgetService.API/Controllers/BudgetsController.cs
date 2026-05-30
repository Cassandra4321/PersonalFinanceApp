using BudgetService.Contracts.Budgets;
using BudgetService.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BudgetService.API.Controllers;

[ApiController]
[Route("api/budgets")]
public sealed class BudgetsController : ControllerBase
{
    private readonly IBudgetRepository _budgetRepository;

    public BudgetsController(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(BudgetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BudgetResponse>> GetByUserId(
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var budget = await _budgetRepository.GetByUserIdAsync(userId, cancellationToken);

        if (budget is null)
            return NotFound();

        return Ok(
            new BudgetResponse
            {
                Id = budget.Id,
                UserId = budget.UserId,
                MonthlyLimit = budget.MonthlyLimit,
                TotalSpent = budget.TotalSpent,
                CreatedAt = budget.CreatedAt,
            }
        );
    }
}
