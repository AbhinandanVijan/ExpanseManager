namespace ExpanseManagerAPI.Entities;

/*
Mask is the last 2–4 digits of the bank account number returned by Plaid.
Eg- Checking •••• 9012
*/

public class BankAccount
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BankConnectionId { get; set; }
    public BankConnection BankConnection { get; set; } = default!;  // Navigation property
    public string ProviderAccountId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Mask { get; set; }

    public string Type { get; set; } = default!;
    public string? Subtype { get; set; }

    public string CurrencyCode { get; set; } = "USD";

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();   // Navigation property
}
