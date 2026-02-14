using System.Security.Claims;
using ExpanseManagerAPI.DTOs;
using ExpanseManagerAPI.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpanseManagerAPI.Controllers;

[ApiController]
[Route("api/transactions")]
// [Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    // GET /api/transactions?bankAccountId=...&from=2026-01-01&to=2026-02-01
    [HttpGet]
    public async Task<ActionResult<List<TransactionDto>>> Get(
        [FromQuery] Guid bankAccountId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        CancellationToken ct)
    {
        var userId = GetUserId();
        var txs = await _transactionService.GetTransactionsAsync(userId, bankAccountId, from, to, ct);

        var result = txs.Select(t => new TransactionDto(
            t.Id,
            t.PostedDate,
            t.Amount,
            t.CurrencyCode,
            t.Name,
            t.MerchantName,
            t.Pending
        )).ToList();

        return Ok(result);
    }

    // POST /api/transactions/sync { "bankAccountId": "..." }
    public record SyncTransactionsRequestDto(Guid BankAccountId);

    [HttpPost("sync")]
    public async Task<ActionResult<SyncTransactionsResponseDto>> Sync(
        [FromBody] SyncTransactionsRequestDto dto,
        CancellationToken ct)
    {
        var userId = GetUserId();
        var syncedCount = await _transactionService.SyncTransactionsAsync(userId, dto.BankAccountId, ct);
        return Ok(new SyncTransactionsResponseDto(syncedCount));
    }

    private Guid GetUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? User.FindFirstValue("sub");

        if (Guid.TryParse(idStr, out var userId))
            return userId;

        throw new UnauthorizedAccessException("UserId claim not found/invalid.");
    }
}
