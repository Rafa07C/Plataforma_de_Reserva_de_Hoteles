using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.Bookings.Commands.ConfirmBooking;

public sealed record ConfirmBookingCommand(Guid BookingId) : IRequest<Result>;
