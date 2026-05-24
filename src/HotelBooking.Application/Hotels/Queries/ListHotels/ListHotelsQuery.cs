using HotelBooking.Application.Common;
using MediatR;

namespace HotelBooking.Application.Hotels.Queries.ListHotels;

public sealed record ListHotelsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SortBy = null,
    string? SortDirection = null) : IRequest<PagedResult<HotelSummaryDto>>;

public sealed class HotelSummaryDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public bool IsActive { get; init; }
}
