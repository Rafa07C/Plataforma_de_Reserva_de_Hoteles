using HotelBooking.Domain.Abstractions;

namespace HotelBooking.Domain.Bookings.Events;

public sealed record BookingConfirmedDomainEvent(BookingId BookingId) : DomainEvent;
