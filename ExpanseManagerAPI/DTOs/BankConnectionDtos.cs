namespace ExpanseManagerAPI.DTOs;

public record CreateLinkTokenResponseDto(
    string LinkToken
);

public record ConnectBankRequestDto(
    string PublicToken
);

public record ConnectBankResponseDto(
    Guid BankConnectionId
);

public record BankConnectionDto(
    Guid Id,
    string Provider,
    DateTime CreatedAt,
    DateTime? LastSyncedAt
);
