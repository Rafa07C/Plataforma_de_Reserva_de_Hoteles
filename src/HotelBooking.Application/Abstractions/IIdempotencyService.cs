namespace HotelBooking.Application.Abstractions;

public interface IIdempotencyService
{
    Task<bool> ExistsAsync(string idempotencyKey, CancellationToken cancellationToken = default);

    Task<string?> GetResponseAsync(string idempotencyKey, CancellationToken cancellationToken = default);

    Task SaveAsync(
        string idempotencyKey,
        string requestHash,
        int responseStatus,
        string responseBody,
        CancellationToken cancellationToken = default);
}
