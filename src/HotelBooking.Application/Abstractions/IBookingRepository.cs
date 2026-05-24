using HotelBooking.Domain.Bookings;

namespace HotelBooking.Application.Abstractions;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(BookingId id, CancellationToken cancellationToken = default);
    Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(BookingId id, CancellationToken cancellationToken = default);
}
