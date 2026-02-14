using System.ComponentModel;
using ExpanseManagerAPI.Data;
using ExpanseManagerAPI.Entities;
using ExpanseManagerAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExpanseManagerAPI.Data.Repositories;

public class BankConnectionRepository : IBankConnectionRepository
{
    private readonly ExpanseDbContext _db;

    public BankConnectionRepository(ExpanseDbContext db) => _db = db;

    public Task AddAsync(BankConnection connection, CancellationToken ct = default)
        => _db.BankConnections.AddAsync(connection, ct).AsTask();

    public Task<BankConnection?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.BankConnections
              .Include(x => x.BankAccounts)
              .FirstOrDefaultAsync(x => x.Id == id, ct);

public async Task<IReadOnlyList<BankConnection>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    => await _db.BankConnections
        .AsNoTracking()
        .Where(x => x.UserId == userId)
        .OrderByDescending(x => x.CreatedAt)
        .ToListAsync(ct);


    public Task SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
