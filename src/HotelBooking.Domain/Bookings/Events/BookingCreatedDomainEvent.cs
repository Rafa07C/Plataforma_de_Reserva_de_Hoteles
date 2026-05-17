using HotelBooking.Domain.Abstractions;

namespace HotelBooking.Domain.Bookings.Events;

public sealed record BookingCreatedDomainEvent(BookingId BookingId) : DomainEvent;
