using ExpanseManagerAPI.Entities;
using ExpanseManagerAPI.Interfaces.Providers;
using ExpanseManagerAPI.Interfaces.Repositories;
using ExpanseManagerAPI.Interfaces.Security;
using ExpanseManagerAPI.Interfaces.Services;

namespace ExpanseManagerAPI.Services;

public class BankConnectionService : IBankConnectionService
{
    private readonly IBankConnectionRepository _connections;
    private readonly IBankDataProviderClient _provider;
    private readonly ITokenEncryptionService _crypto;

    public BankConnectionService(
        IBankConnectionRepository connections,
        IBankDataProviderClient provider,
        ITokenEncryptionService crypto)
    {
        _connections = connections;
        _provider = provider;
        _crypto = crypto;
    }

    public Task<string> CreateLinkTokenAsync(Guid userId, CancellationToken ct = default)
        => _provider.CreateLinkTokenAsync(userId, ct);

    public async Task<Guid> ConnectAsync(Guid userId, string publicToken, CancellationToken ct = default)
    {
        var (accessToken, itemId) = await _provider.ExchangePublicTokenAsync(publicToken, ct);

        var connection = new BankConnection
        {
            UserId = userId,
            Provider = "Plaid",
            ProviderItemId = itemId,
            EncryptedAccessToken = _crypto.Encrypt(accessToken),
            CreatedAt = DateTime.UtcNow
        };

        await _connections.AddAsync(connection, ct);

        // Optional: import accounts right away
        var accounts = await _provider.GetAccountsAsync(accessToken, ct);
        foreach (var a in accounts)
        {
            connection.BankAccounts.Add(new BankAccount
            {
                BankConnectionId = connection.Id,
                ProviderAccountId = a.ProviderAccountId,
                Name = a.Name,
                Mask = a.Mask,
                Type = a.Type,
                Subtype = a.Subtype,
                CurrencyCode = string.IsNullOrWhiteSpace(a.CurrencyCode) ? "USD" : a.CurrencyCode
            });
        }

        await _connections.SaveChangesAsync(ct);
        return connection.Id;
    }
}
