using HotelBooking.Application.Abstractions;
using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.Availability.Queries.GetAvailability;

public sealed class GetAvailabilityQueryHandler
    : IRequestHandler<GetAvailabilityQuery, Result<AvailabilityDto>>
{
    private readonly IReadDbConnection _db;

    public GetAvailabilityQueryHandler(IReadDbConnection db)
    {
        _db = db;
    }

    public async Task<Result<AvailabilityDto>> Handle(
        GetAvailabilityQuery request,
        CancellationToken cancellationToken)
    {
        const string hotelSql = """
            SELECT Id, Name
            FROM Hotels
            WHERE Id = @HotelId AND IsActive = 1
            """;

        var hotel = await _db.QueryFirstOrDefaultAsync<dynamic>(
            hotelSql,
            new { request.HotelId },
            cancellationToken: cancellationToken);

        if (hotel is null)
            return DomainError.NotFound(
                "Hotel.NotFound",
                "Hotel not found");

        // Finds room types where ALL nights in the requested range have availability.
        // MIN(AvailableRooms) ensures we only return rooms available for every night.
        const string availabilitySql = """
            SELECT
                rt.Id AS RoomTypeId,
                rt.Name AS RoomTypeName,
                rt.Description,
                rt.MaxGuests,
                rp.PricePerNight,
                rp.PricePerNight * DATEDIFF(day, @CheckIn, @CheckOut) AS TotalPrice,
                rp.Currency,
                MIN(ri.AvailableRooms) AS AvailableRooms
            FROM RoomTypes rt
            INNER JOIN RoomInventory ri ON ri.RoomTypeId = rt.Id
            INNER JOIN RatePlans rp ON rp.RoomTypeId = rt.Id
                AND rp.IsActive = 1
                AND rp.ValidFrom <= @CheckIn
                AND rp.ValidTo >= @CheckOut
            WHERE rt.HotelId = @HotelId
                AND rt.IsActive = 1
                AND rt.MaxGuests >= @GuestsCount
                AND ri.Date >= @CheckIn
                AND ri.Date < @CheckOut
            GROUP BY
                rt.Id, rt.Name, rt.Description, rt.MaxGuests,
                rp.PricePerNight, rp.Currency
            HAVING MIN(ri.AvailableRooms) > 0
            ORDER BY rp.PricePerNight ASC
            """;

        var availableRooms = await _db.QueryAsync<AvailableRoomDto>(
            availabilitySql,
            new
            {
                request.HotelId,
                request.CheckIn,
                request.CheckOut,
                request.GuestsCount
            },
            cancellationToken: cancellationToken);

        var nights = request.CheckOut.DayNumber - request.CheckIn.DayNumber;

        return new AvailabilityDto
        {
            HotelId = request.HotelId,
            HotelName = hotel.Name,
            CheckIn = request.CheckIn,
            CheckOut = request.CheckOut,
            Nights = nights,
            AvailableRooms = availableRooms.ToList()
        };
    }
}
