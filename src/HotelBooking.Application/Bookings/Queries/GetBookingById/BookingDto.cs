namespace HotelBooking.Application.Bookings.Queries.GetBookingById;

public sealed class BookingDto
{
    public Guid Id { get; init; }
    public Guid HotelId { get; init; }
    public string HotelName { get; init; } = string.Empty;
    public Guid RoomTypeId { get; init; }
    public string RoomTypeName { get; init; } = string.Empty;
    public DateOnly CheckIn { get; init; }
    public DateOnly CheckOut { get; init; }
    public int Nights { get; init; }
    public int GuestsCount { get; init; }
    public decimal TotalPrice { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? ConfirmedAt { get; init; }
    public DateTime? CancelledAt { get; init; }
    public DateTime? ExpiresAt { get; init; }
    public string? CancellationReason { get; init; }
    public List<GuestDto> Guests { get; init; } = [];
}

public sealed class GuestDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
}
