namespace ExpanseManagerAPI.Interfaces.Services;

public interface IBankConnectionService
{
    Task<string> CreateLinkTokenAsync(Guid userId, CancellationToken ct = default);
    Task<Guid> ConnectAsync(Guid userId, string publicToken, CancellationToken ct = default);
}
