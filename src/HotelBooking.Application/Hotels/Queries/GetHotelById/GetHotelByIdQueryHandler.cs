using HotelBooking.Application.Abstractions;
using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.Hotels.Queries.GetHotelById;

public sealed class GetHotelByIdQueryHandler
    : IRequestHandler<GetHotelByIdQuery, Result<HotelDto>>
{
    private readonly IReadDbConnection _db;

    public GetHotelByIdQueryHandler(IReadDbConnection db)
    {
        _db = db;
    }

    public async Task<Result<HotelDto>> Handle(
        GetHotelByIdQuery request,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT 
                h.Id, h.Name, h.Address, h.City, h.Country, 
                h.IsActive, h.CreatedAt,
                rt.Id, rt.Name, rt.Description, 
                rt.MaxGuests, rt.BasePrice, rt.Currency
            FROM Hotels h
            LEFT JOIN RoomTypes rt ON rt.HotelId = h.Id AND rt.IsActive = 1
            WHERE h.Id = @HotelId AND h.IsActive = 1
            """;

        var hotel = await _db.QueryFirstOrDefaultAsync<HotelDto>(
            sql,
            new { HotelId = request.HotelId },
            cancellationToken: cancellationToken);

        if (hotel is null)
            return DomainError.NotFound(
                "Hotel.NotFound",
                "Hotel not found");

        return hotel;
    }
}
