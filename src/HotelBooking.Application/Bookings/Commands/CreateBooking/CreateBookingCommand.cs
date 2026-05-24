using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.Bookings.Commands.CreateBooking;

public sealed record CreateBookingCommand(
    Guid HotelId,
    Guid RoomTypeId,
    Guid RatePlanId,
    DateOnly CheckIn,
    DateOnly CheckOut,
    int GuestsCount,
    string GuestFirstName,
    string GuestLastName,
    string GuestEmail,
    string GuestPhone,
    string? IdempotencyKey = null) : IRequest<Result<Guid>>;
