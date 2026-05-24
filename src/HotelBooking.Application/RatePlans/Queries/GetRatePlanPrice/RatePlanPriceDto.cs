namespace HotelBooking.Application.RatePlans.Queries.GetRatePlanPrice;

public sealed class RatePlanPriceDto
{
    public Guid RatePlanId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal PricePerNight { get; init; }
    public decimal TotalPrice { get; init; }
    public string Currency { get; init; } = string.Empty;
    public int Nights { get; init; }
    public DateOnly ValidFrom { get; init; }
    public DateOnly ValidTo { get; init; }
}
