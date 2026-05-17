using HotelBooking.Domain.Abstractions;

namespace HotelBooking.Domain.Bookings.Events;

public sealed record BookingCancelledDomainEvent(BookingId BookingId) : DomainEvent;
