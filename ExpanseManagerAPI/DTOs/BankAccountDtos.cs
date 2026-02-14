namespace ExpanseManagerAPI.DTOs;

public record BankAccountDto(
    Guid Id,
    string Name,
    string? Mask,
    string Type,
    string? Subtype,
    string CurrencyCode
);
