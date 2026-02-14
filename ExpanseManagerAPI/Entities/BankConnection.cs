/*
BankConnection stores the mapping between your user and Plaid’s item_id + access_token,
so your system knows which bank connection belongs to which user.
Plaid handles bank authentication, but your app must persist the connection,
manage security, and control future data syncs.
*/

namespace ExpanseManagerAPI.Entities;

public class BankConnection
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;  // Navigation property
    public string Provider { get; set; } = "Plaid";
    public string ProviderItemId { get; set; } = default!;
    public string EncryptedAccessToken { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastSyncedAt { get; set; }
    public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
}
