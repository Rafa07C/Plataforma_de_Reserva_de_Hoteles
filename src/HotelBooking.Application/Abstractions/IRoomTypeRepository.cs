using HotelBooking.Domain.Hotels;

namespace HotelBooking.Application.Abstractions;

public interface IRoomTypeRepository
{
    Task<RoomType?> GetByIdAsync(RoomTypeId id, CancellationToken cancellationToken = default);
    Task AddAsync(RoomType roomType, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(RoomTypeId id, CancellationToken cancellationToken = default);
}
