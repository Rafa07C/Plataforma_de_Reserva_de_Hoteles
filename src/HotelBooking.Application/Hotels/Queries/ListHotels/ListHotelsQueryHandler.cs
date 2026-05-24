using HotelBooking.Application.Abstractions;
using HotelBooking.Application.Common;
using MediatR;

namespace HotelBooking.Application.Hotels.Queries.ListHotels;

public sealed class ListHotelsQueryHandler
    : IRequestHandler<ListHotelsQuery, PagedResult<HotelSummaryDto>>
{
    private readonly IReadDbConnection _db;

    public ListHotelsQueryHandler(IReadDbConnection db)
    {
        _db = db;
    }

    public async Task<PagedResult<HotelSummaryDto>> Handle(
        ListHotelsQuery request,
        CancellationToken cancellationToken)
    {
        var offset = (request.PageNumber - 1) * request.PageSize;

        var allowedSortColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Name", "City", "Country", "CreatedAt"
        };

        var sortBy = allowedSortColumns.Contains(request.SortBy ?? string.Empty)
            ? request.SortBy
            : "Name";

        var sortDirection = request.SortDirection?.ToUpperInvariant() == "DESC"
            ? "DESC"
            : "ASC";

        var sql = $"""
            SELECT Id, Name, City, Country, IsActive
            FROM Hotels
            WHERE IsActive = 1
            ORDER BY {sortBy} {sortDirection}
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*) FROM Hotels WHERE IsActive = 1;
            """;

        var hotels = await _db.QueryAsync<HotelSummaryDto>(
            sql,
            new { Offset = offset, request.PageSize },
            cancellationToken: cancellationToken);

        var countSql = "SELECT COUNT(*) FROM Hotels WHERE IsActive = 1";
        var totalRecords = await _db.QueryFirstOrDefaultAsync<int>(
            countSql,
            cancellationToken: cancellationToken);

        return PagedResult.Create(
            hotels.ToList(),
            request.PageNumber,
            request.PageSize,
            totalRecords);
    }
}
