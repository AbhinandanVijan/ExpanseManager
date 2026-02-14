using ExpanseManagerAPI.Entities;

namespace ExpanseManagerAPI.Interfaces.Repositories;

public interface ITransactionRepository
{
    Task AddRangeAsync(IEnumerable<Transaction> transactions, CancellationToken ct = default);

    Task<IReadOnlyList<Transaction>> GetByAccountIdAsync(
        Guid bankAccountId,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken ct = default);

    Task<bool> ExistsAsync(Guid bankAccountId, string providerTransactionId, CancellationToken ct = default);

    Task<DateTime?> GetLatestPostedDateAsync(Guid bankAccountId, CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}
