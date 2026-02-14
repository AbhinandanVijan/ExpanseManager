using ExpanseManagerAPI.Data;
using ExpanseManagerAPI.Entities;
using ExpanseManagerAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExpanseManagerAPI.Data.Repositories;

public class BankAccountRepository : IBankAccountRepository
{
    private readonly ExpanseDbContext _db;

    public BankAccountRepository(ExpanseDbContext db) => _db = db;

    public Task AddRangeAsync(IEnumerable<BankAccount> accounts, CancellationToken ct = default)
        => _db.BankAccounts.AddRangeAsync(accounts, ct);

    public async Task<IReadOnlyList<BankAccount>> GetByConnectionIdAsync(Guid bankConnectionId, CancellationToken ct = default)
        => await _db.BankAccounts
              .Where(a => a.BankConnectionId == bankConnectionId)
              .OrderBy(a => a.Name)
              .ToListAsync(ct);

    public Task<BankAccount?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.BankAccounts.FirstOrDefaultAsync(a => a.Id == id, ct);

    public Task<bool> ExistsAsync(Guid bankConnectionId, string providerAccountId, CancellationToken ct = default)
        => _db.BankAccounts.AnyAsync(
            a => a.BankConnectionId == bankConnectionId && a.ProviderAccountId == providerAccountId,
            ct);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
