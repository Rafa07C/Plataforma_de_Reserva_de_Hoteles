using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.Bookings.Queries.GetBookingById;

public sealed record GetBookingByIdQuery(Guid BookingId) : IRequest<Result<BookingDto>>;
