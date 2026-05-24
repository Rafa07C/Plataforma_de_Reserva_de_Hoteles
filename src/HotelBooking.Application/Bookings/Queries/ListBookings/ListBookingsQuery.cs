using HotelBooking.Application.Common;
using MediatR;

namespace HotelBooking.Application.Bookings.Queries.ListBookings;

public sealed record ListBookingsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SortBy = null,
    string? SortDirection = null,
    string? Status = null,
    Guid? HotelId = null) : IRequest<PagedResult<BookingSummaryDto>>;

public sealed class BookingSummaryDto
{
    public Guid Id { get; init; }
    public string HotelName { get; init; } = string.Empty;
    public string RoomTypeName { get; init; } = string.Empty;
    public DateOnly CheckIn { get; init; }
    public DateOnly CheckOut { get; init; }
    public int GuestsCount { get; init; }
    public decimal TotalPrice { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
