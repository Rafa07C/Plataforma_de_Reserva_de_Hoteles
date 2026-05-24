using System.Data;

namespace HotelBooking.Application.Abstractions;

/// <summary>
/// Abstraction over the read-side database connection (Dapper).
/// Keeps Application layer free of any SQL Server or ADO.NET dependencies.
/// </summary>
public interface IReadDbConnection
{
    Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        CancellationToken cancellationToken = default);

    Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        CancellationToken cancellationToken = default);

    Task<int> ExecuteAsync(
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        CancellationToken cancellationToken = default);
}
