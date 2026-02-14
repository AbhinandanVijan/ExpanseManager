using ExpanseManagerAPI.Entities;

namespace ExpanseManagerAPI.Interfaces.Services;

public interface ITransactionService
{
    Task<IReadOnlyList<Transaction>> GetTransactionsAsync(
        Guid userId,
        Guid bankAccountId,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken ct = default);

    Task<int> SyncTransactionsAsync(
        Guid userId,
        Guid bankAccountId,
        CancellationToken ct = default);
}
