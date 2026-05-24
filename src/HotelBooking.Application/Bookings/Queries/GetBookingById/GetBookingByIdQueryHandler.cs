using HotelBooking.Application.Abstractions;
using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.Bookings.Queries.GetBookingById;

public sealed class GetBookingByIdQueryHandler
    : IRequestHandler<GetBookingByIdQuery, Result<BookingDto>>
{
    private readonly IReadDbConnection _db;

    public GetBookingByIdQueryHandler(IReadDbConnection db)
    {
        _db = db;
    }

    public async Task<Result<BookingDto>> Handle(
        GetBookingByIdQuery request,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                b.Id, b.HotelId, h.Name AS HotelName,
                b.RoomTypeId, rt.Name AS RoomTypeName,
                b.CheckIn, b.CheckOut,
                DATEDIFF(day, b.CheckIn, b.CheckOut) AS Nights,
                b.GuestsCount, b.TotalPrice, b.Currency,
                b.Status, b.CreatedAt, b.ConfirmedAt,
                b.CancelledAt, b.ExpiresAt, b.CancellationReason,
                g.Id, g.FirstName, g.LastName, g.Email, g.Phone
            FROM Bookings b
            INNER JOIN Hotels h ON h.Id = b.HotelId
            INNER JOIN RoomTypes rt ON rt.Id = b.RoomTypeId
            LEFT JOIN Guests g ON g.BookingId = b.Id
            WHERE b.Id = @BookingId
            """;

        var booking = await _db.QueryFirstOrDefaultAsync<BookingDto>(
            sql,
            new { request.BookingId },
            cancellationToken: cancellationToken);

        if (booking is null)
            return DomainError.NotFound(
                "Booking.NotFound",
                "Booking not found");

        return booking;
    }
}
