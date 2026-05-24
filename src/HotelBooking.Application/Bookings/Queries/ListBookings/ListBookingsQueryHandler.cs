using HotelBooking.Application.Abstractions;
using HotelBooking.Application.Common;
using MediatR;

namespace HotelBooking.Application.Bookings.Queries.ListBookings;

public sealed class ListBookingsQueryHandler
    : IRequestHandler<ListBookingsQuery, PagedResult<BookingSummaryDto>>
{
    private readonly IReadDbConnection _db;

    public ListBookingsQueryHandler(IReadDbConnection db)
    {
        _db = db;
    }

    public async Task<PagedResult<BookingSummaryDto>> Handle(
        ListBookingsQuery request,
        CancellationToken cancellationToken)
    {
        var offset = (request.PageNumber - 1) * request.PageSize;

        var allowedSortColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "CreatedAt", "CheckIn", "CheckOut", "Status", "TotalPrice"
        };

        var sortBy = allowedSortColumns.Contains(request.SortBy ?? string.Empty)
            ? request.SortBy
            : "CreatedAt";

        var sortDirection = request.SortDirection?.ToUpperInvariant() == "DESC"
            ? "DESC"
            : "ASC";

        var whereClause = "WHERE 1=1";
        if (!string.IsNullOrEmpty(request.Status))
            whereClause += " AND b.Status = @Status";
        if (request.HotelId.HasValue)
            whereClause += " AND b.HotelId = @HotelId";

        var sql = $"""
            SELECT
                b.Id, h.Name AS HotelName, rt.Name AS RoomTypeName,
                b.CheckIn, b.CheckOut, b.GuestsCount,
                b.TotalPrice, b.Currency, b.Status, b.CreatedAt
            FROM Bookings b
            INNER JOIN Hotels h ON h.Id = b.HotelId
            INNER JOIN RoomTypes rt ON rt.Id = b.RoomTypeId
            {whereClause}
            ORDER BY b.{sortBy} {sortDirection}
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
            """;

        var countSql = $"""
            SELECT COUNT(*)
            FROM Bookings b
            {whereClause}
            """;

        var bookings = await _db.QueryAsync<BookingSummaryDto>(
            sql,
            new { Offset = offset, request.PageSize, request.Status, request.HotelId },
            cancellationToken: cancellationToken);

        var totalRecords = await _db.QueryFirstOrDefaultAsync<int>(
            countSql,
            new { request.Status, request.HotelId },
            cancellationToken: cancellationToken);

        return PagedResult.Create(
            bookings.ToList(),
            request.PageNumber,
            request.PageSize,
            totalRecords);
    }
}
