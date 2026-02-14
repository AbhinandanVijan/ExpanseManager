using System.Security.Claims;
using ExpanseManagerAPI.DTOs;
using ExpanseManagerAPI.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpanseManagerAPI.Controllers;

[ApiController]
[Route("api/bank-accounts")]
// [Authorize]
public class BankAccountsController : ControllerBase
{
    private readonly IBankAccountRepository _bankAccountRepo;
    private readonly IBankConnectionRepository _bankConnectionRepo;

    public BankAccountsController(
        IBankAccountRepository bankAccountRepo,
        IBankConnectionRepository bankConnectionRepo)
    {
        _bankAccountRepo = bankAccountRepo;
        _bankConnectionRepo = bankConnectionRepo;
    }

    // GET /api/bank-accounts/by-connection/{bankConnectionId}
    [HttpGet("by-connection/{bankConnectionId:guid}")]
    public async Task<ActionResult<List<BankAccountDto>>> GetByConnection(Guid bankConnectionId, CancellationToken ct)
    {
        var userId = GetUserId();

        // Ownership check: connection must belong to user
        var conn = await _bankConnectionRepo.GetByIdAsync(bankConnectionId, ct);
        if (conn is null) return NotFound();
        if (conn.UserId != userId) return Forbid();

        var accounts = await _bankAccountRepo.GetByConnectionIdAsync(bankConnectionId, ct);

        var result = accounts.Select(a => new BankAccountDto(
            a.Id,
            a.Name,
            a.Mask,
            a.Type,
            a.Subtype,
            a.CurrencyCode
        )).ToList();

        return Ok(result);
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
