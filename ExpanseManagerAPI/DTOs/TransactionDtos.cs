namespace ExpanseManagerAPI.DTOs;

public record TransactionDto(
    Guid Id,
    DateTime PostedDate,
    decimal Amount,
    string CurrencyCode,
    string Name,
    string? MerchantName,
    bool Pending
);

public record GetTransactionsRequestDto(
    Guid BankAccountId,
    DateTime? From,
    DateTime? To
);

public record SyncTransactionsResponseDto(
    int SyncedCount
);