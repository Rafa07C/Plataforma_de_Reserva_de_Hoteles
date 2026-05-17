namespace HotelBooking.Domain.Bookings;

public record struct BookingId(Guid Value)
{
    public static BookingId New() => new(Guid.NewGuid());
    public static BookingId Empty => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}
