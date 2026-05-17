namespace HotelBooking.Domain.Hotels;

public record struct RatePlanId(Guid Value)
{
    public static RatePlanId New() => new(Guid.NewGuid());
    public static RatePlanId Empty => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}
