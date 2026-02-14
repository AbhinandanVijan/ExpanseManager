using ExpanseManagerAPI.Data;
using ExpanseManagerAPI.Entities;
using ExpanseManagerAPI.Interfaces.Repositories;
using ExpanseManagerAPI.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace ExpanseManagerAPI.Services;

public class TransactionService : ITransactionService
{
    private readonly ExpanseDbContext _db;
    private readonly ITransactionRepository _txRepo;

    public TransactionService(ExpanseDbContext db, ITransactionRepository txRepo)
    {
        _db = db;
        _txRepo = txRepo;
    }

    public async Task<IReadOnlyList<Transaction>> GetTransactionsAsync(
        Guid userId,
        Guid bankAccountId,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken ct = default)
    {
        // Security: verify the account belongs to this user via relationship chain
        var isOwned = await _db.BankAccounts
            .Where(a => a.Id == bankAccountId)
            .AnyAsync(a => a.BankConnection.UserId == userId, ct);

        if (!isOwned)
            throw new UnauthorizedAccessException("Bank account does not belong to the user.");

        return await _txRepo.GetByAccountIdAsync(bankAccountId, from, to, ct);
    }

    public async Task<int> SyncTransactionsAsync(Guid userId, Guid bankAccountId, CancellationToken ct = default)
    {
        // Scaffold: You will call provider here later (Plaid Transactions API)
        // Steps will be:
        // 1) Verify ownership (same as GetTransactionsAsync)
        // 2) Find BankConnection + decrypt access token
        // 3) Call provider GetTransactions(accessToken, dateRange)
        // 4) Upsert by ProviderTransactionId (dedupe)
        // 5) Save

        var isOwned = await _db.BankAccounts
            .Where(a => a.Id == bankAccountId)
            .AnyAsync(a => a.BankConnection.UserId == userId, ct);

        if (!isOwned)
            throw new UnauthorizedAccessException("Bank account does not belong to the user.");

        // For now, no provider integration implemented => nothing synced
        return 0;
    }
}
