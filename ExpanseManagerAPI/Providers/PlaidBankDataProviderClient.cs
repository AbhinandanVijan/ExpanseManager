using ExpanseManagerAPI.Interfaces.Providers;

namespace ExpanseManagerAPI.Providers;

public class PlaidBankDataProviderClient : IBankDataProviderClient
{
    public Task<string> CreateLinkTokenAsync(Guid userId, CancellationToken ct = default)
    {
        // TODO: call Plaid LinkTokenCreate and return link_token
        return Task.FromResult("dummy-link-token");
    }

    public Task<(string AccessToken, string ItemId)> ExchangePublicTokenAsync(string publicToken, CancellationToken ct = default)
    {
        // TODO: call Plaid ItemPublicTokenExchange
        return Task.FromResult(("dummy-access-token", "dummy-item-id"));
    }

    public Task<IReadOnlyList<ProviderAccountDto>> GetAccountsAsync(string accessToken, CancellationToken ct = default)
    {
        // TODO: call Plaid AccountsGet and map to ProviderAccountDto
        IReadOnlyList<ProviderAccountDto> result =
        [
            new ProviderAccountDto("acc-1", "Checking", "9012", "depository", "checking", "USD")
        ];
        return Task.FromResult(result);
    }
}
