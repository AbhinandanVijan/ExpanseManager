using ExpanseManagerAPI.Entities;

namespace ExpanseManagerAPI.Interfaces.Repositories;

public interface IBankAccountRepository
{
    Task AddRangeAsync(IEnumerable<BankAccount> accounts, CancellationToken ct = default);
    Task<IReadOnlyList<BankAccount>> GetByConnectionIdAsync(Guid bankConnectionId, CancellationToken ct = default);
    Task<BankAccount?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<bool> ExistsAsync(Guid bankConnectionId, string providerAccountId, CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}
