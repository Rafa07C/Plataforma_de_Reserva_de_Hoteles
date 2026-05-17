namespace HotelBooking.Domain.Hotels;

public record struct RoomInventoryId(Guid Value)
{
    public static RoomInventoryId New() => new(Guid.NewGuid());
    public static RoomInventoryId Empty => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}
