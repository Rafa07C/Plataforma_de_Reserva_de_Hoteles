using HotelBooking.Domain.Hotels;

namespace HotelBooking.Application.Abstractions;

public interface IHotelRepository
{
    Task<Hotel?> GetByIdAsync(HotelId id, CancellationToken cancellationToken = default);
    Task AddAsync(Hotel hotel, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(HotelId id, CancellationToken cancellationToken = default);
}
