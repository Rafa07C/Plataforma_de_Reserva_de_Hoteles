namespace HotelBooking.Domain.Hotels;

public record struct RoomTypeId(Guid Value)
{
    public static RoomTypeId New() => new(Guid.NewGuid());
    public static RoomTypeId Empty => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}
