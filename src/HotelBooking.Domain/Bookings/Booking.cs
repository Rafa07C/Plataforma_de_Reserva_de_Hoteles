using HotelBooking.Domain.Abstractions;
using HotelBooking.Domain.Bookings.Events;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Enums;
using HotelBooking.Domain.Hotels;
using HotelBooking.Domain.ValueObjects;

namespace HotelBooking.Domain.Bookings;

/// <summary>
/// Aggregate root for the booking lifecycle.
/// Handles state transitions and raises domain events on each change.
/// </summary>
public sealed class Booking : Entity<BookingId>, IAggregateRoot
{
    private readonly List<Guest> _guests = [];

    private Booking(
        BookingId id,
        HotelId hotelId,
        RoomTypeId roomTypeId,
        RatePlanId ratePlanId,
        DateRange dateRange,
        GuestsCount guestsCount,
        Money totalPrice) : base(id)
    {
        HotelId = hotelId;
        RoomTypeId = roomTypeId;
        RatePlanId = ratePlanId;
        DateRange = dateRange;
        GuestsCount = guestsCount;
        TotalPrice = totalPrice;
    }

    public HotelId HotelId { get; private set; }
    public RoomTypeId RoomTypeId { get; private set; }
    public RatePlanId RatePlanId { get; private set; }
    public DateRange DateRange { get; private set; }
    public GuestsCount GuestsCount { get; private set; }
    public Money TotalPrice { get; private set; }
    public BookingStatus Status { get; private set; } = BookingStatus.Pending;
    public DateTime CreatedAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    /// <summary>
    /// Pending bookings auto-expire if not confirmed within the allowed window.
    /// The background worker checks this field to mark bookings as Expired.
    /// </summary>
    public DateTime? ExpiresAt { get; private set; }
    public string? CancellationReason { get; private set; }

    public IReadOnlyList<Guest> Guests => _guests.AsReadOnly();

    public static Result<Booking> Create(
        HotelId hotelId,
        RoomTypeId roomTypeId,
        RatePlanId ratePlanId,
        DateRange dateRange,
        GuestsCount guestsCount,
        Money totalPrice,
        int expirationMinutes = 30)
    {
        var booking = new Booking(
            BookingId.New(),
            hotelId,
            roomTypeId,
            ratePlanId,
            dateRange,
            guestsCount,
            totalPrice)
        {
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
        };

        booking.AddDomainEvent(new BookingCreatedDomainEvent(booking.Id));

        return booking;
    }

    public Result AddGuest(Guest guest)
    {
        if (_guests.Count >= GuestsCount.Value)
            return DomainError.Conflict(
                "Booking.GuestLimitReached",
                "Cannot add more guests than the booking capacity");

        _guests.Add(guest);
        return Result.Success();
    }

    /// <summary>
    /// Valid transitions: Pending → Confirmed.
    /// Raises BookingConfirmedDomainEvent on success.
    /// </summary>
    public Result Confirm()
    {
        if (Status == BookingStatus.Confirmed)
            return DomainError.Conflict(
                "Booking.AlreadyConfirmed",
                "Booking is already confirmed");

        if (Status == BookingStatus.Cancelled)
            return DomainError.Conflict(
                "Booking.AlreadyCancelled",
                "Cannot confirm a cancelled booking");

        if (Status == BookingStatus.Expired)
            return DomainError.Conflict(
                "Booking.Expired",
                "Cannot confirm an expired booking");

        if (DateTime.UtcNow > ExpiresAt)
            return DomainError.Conflict(
                "Booking.Expired",
                "Booking has expired");

        Status = BookingStatus.Confirmed;
        ConfirmedAt = DateTime.UtcNow;

        AddDomainEvent(new BookingConfirmedDomainEvent(Id));

        return Result.Success();
    }

    /// <summary>
    /// Valid transitions: Pending → Cancelled, Confirmed → Cancelled.
    /// Raises BookingCancelledDomainEvent on success.
    /// </summary>
    public Result Cancel(string? reason = null)
    {
        if (Status == BookingStatus.Cancelled)
            return DomainError.Conflict(
                "Booking.AlreadyCancelled",
                "Booking is already cancelled");

        Status = BookingStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
        CancellationReason = reason;

        AddDomainEvent(new BookingCancelledDomainEvent(Id));

        return Result.Success();
    }

    /// <summary>
    /// Valid transitions: Pending → Expired.
    /// Called by the background expiration worker, not by user action.
    /// </summary>
    public Result Expire()
    {
        if (Status != BookingStatus.Pending)
            return DomainError.Conflict(
                "Booking.CannotExpire",
                "Only pending bookings can expire");

        Status = BookingStatus.Expired;
        return Result.Success();
    }
}
