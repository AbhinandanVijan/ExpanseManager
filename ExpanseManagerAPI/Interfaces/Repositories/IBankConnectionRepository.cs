using ExpanseManagerAPI.Entities;

namespace ExpanseManagerAPI.Interfaces.Repositories;

public interface IBankConnectionRepository
{
    Task AddAsync(BankConnection connection, CancellationToken ct = default);
    Task<BankConnection?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<BankConnection>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
