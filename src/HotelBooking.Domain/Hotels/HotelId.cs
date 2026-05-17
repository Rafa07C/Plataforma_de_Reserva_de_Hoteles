namespace HotelBooking.Domain.Hotels;

public record struct HotelId(Guid Value)
{
    public static HotelId New() => new(Guid.NewGuid());
    public static HotelId Empty => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}
