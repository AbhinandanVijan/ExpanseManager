using System.Security.Claims;
using ExpanseManagerAPI.DTOs;
using ExpanseManagerAPI.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpanseManagerAPI.Controllers;

[ApiController]
[Route("api/bank-connections")]
// [Authorize] // enable when JWT auth is ready
public class BankConnectionsController : ControllerBase
{
    private readonly IBankConnectionService _bankConnectionService;

    public BankConnectionsController(IBankConnectionService bankConnectionService)
    {
        _bankConnectionService = bankConnectionService;
    }

    [HttpPost("link-token")]
    public async Task<ActionResult<CreateLinkTokenResponseDto>> CreateLinkToken(CancellationToken ct)
    {
        var userId = GetUserId();
        var linkToken = await _bankConnectionService.CreateLinkTokenAsync(userId, ct);
        return Ok(new CreateLinkTokenResponseDto(linkToken));
    }

    [HttpPost("connect")]
    public async Task<ActionResult<ConnectBankResponseDto>> Connect([FromBody] ConnectBankRequestDto dto, CancellationToken ct)
    {
        var userId = GetUserId();
        var connectionId = await _bankConnectionService.ConnectAsync(userId, dto.PublicToken, ct);
        return Ok(new ConnectBankResponseDto(connectionId));
    }

    private Guid GetUserId()
    {
        // Typical JWT claim
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? User.FindFirstValue("sub");

        if (Guid.TryParse(idStr, out var userId))
            return userId;

        // TEMP for local testing without auth (replace/remove later)
        // return Guid.Parse("11111111-1111-1111-1111-111111111111");

        throw new UnauthorizedAccessException("UserId claim not found/invalid. Enable auth or provide a test userId.");
    }
}
