namespace ExpanseManagerAPI.Entities;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BankAccountId { get; set; }
    public BankAccount BankAccount { get; set; } = default!;  // Navigation property
    public string ProviderTransactionId { get; set; } = default!;
    public DateTime PostedDate { get; set; }

    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; } = "USD";

    public string Name { get; set; } = default!;
    public string? MerchantName { get; set; }

    public bool Pending { get; set; }
}
