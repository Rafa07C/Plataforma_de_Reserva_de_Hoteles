using HotelBooking.Domain.Hotels;

namespace HotelBooking.Application.Abstractions;

public interface IRoomInventoryRepository
{
    Task<IReadOnlyList<RoomInventory>> GetByRoomTypeAndDateRangeAsync(
        RoomTypeId roomTypeId,
        DateOnly checkIn,
        DateOnly checkOut,
        CancellationToken cancellationToken = default);

    Task<bool> HasAvailabilityAsync(
        RoomTypeId roomTypeId,
        DateOnly checkIn,
        DateOnly checkOut,
        CancellationToken cancellationToken = default);

    Task AddRangeAsync(
        IEnumerable<RoomInventory> inventories,
        CancellationToken cancellationToken = default);
}
