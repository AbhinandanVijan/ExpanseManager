using Going.Plaid;
using Going.Plaid.Entity;
using Going.Plaid.Link;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/plaid")]
public class PlaidTestController : ControllerBase
{
    private readonly PlaidClient _plaid;

    public PlaidTestController(PlaidClient plaid)
    {
        _plaid = plaid;
    }

    [HttpPost("link-token")]
    public async Task<IActionResult> CreateLinkToken()
    {
        var request = new LinkTokenCreateRequest
        {
            User = new LinkTokenCreateRequestUser
            {
                ClientUserId = Guid.NewGuid().ToString()
            },
            ClientName = "ExpanseManager",
            Products = new[] { Products.Transactions },
            CountryCodes = new[] { CountryCode.Us },
            Language = Language.English
        };

        var response = await _plaid.LinkTokenCreateAsync(request);

        return Ok(response.LinkToken);
    }
}
