using ExpanseManagerAPI.Data;
using ExpanseManagerAPI.Entities;
using ExpanseManagerAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExpanseManagerAPI.Data.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ExpanseDbContext _db;

    public TransactionRepository(ExpanseDbContext db) => _db = db;

    public Task AddRangeAsync(IEnumerable<Transaction> transactions, CancellationToken ct = default)
        => _db.Transactions.AddRangeAsync(transactions, ct);

    public async Task<IReadOnlyList<Transaction>> GetByAccountIdAsync(
        Guid bankAccountId,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken ct = default)
    {
        var q = _db.Transactions.AsQueryable()
            .Where(t => t.BankAccountId == bankAccountId);

        if (from.HasValue) q = q.Where(t => t.PostedDate >= from.Value);
        if (to.HasValue) q = q.Where(t => t.PostedDate <= to.Value);

        return (await q.OrderByDescending(t => t.PostedDate).ToListAsync(ct)).AsReadOnly();
    }

    public Task<bool> ExistsAsync(Guid bankAccountId, string providerTransactionId, CancellationToken ct = default)
        => _db.Transactions.AnyAsync(
            t => t.BankAccountId == bankAccountId && t.ProviderTransactionId == providerTransactionId,
            ct);

    public async Task<DateTime?> GetLatestPostedDateAsync(Guid bankAccountId, CancellationToken ct = default)
    {
        return await _db.Transactions
            .Where(t => t.BankAccountId == bankAccountId)
            .MaxAsync(t => (DateTime?)t.PostedDate, ct);
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
