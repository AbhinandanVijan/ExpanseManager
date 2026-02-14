namespace ExpanseManagerAPI.Interfaces.Providers;

public record ProviderAccountDto(   // what is recodd in c#? https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record
    string ProviderAccountId,
    string Name,
    string? Mask,
    string Type,
    string? Subtype,
    string CurrencyCode
);

public interface IBankDataProviderClient
{
    Task<string> CreateLinkTokenAsync(Guid userId, CancellationToken ct = default);

    Task<(string AccessToken, string ItemId)> ExchangePublicTokenAsync(
        string publicToken,
        CancellationToken ct = default);

    Task<IReadOnlyList<ProviderAccountDto>> GetAccountsAsync(
        string accessToken,
        CancellationToken ct = default);
}
