using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.Bookings.Commands.CancelBooking;

public sealed record CancelBookingCommand(
    Guid BookingId,
    string? Reason = null) : IRequest<Result>;
