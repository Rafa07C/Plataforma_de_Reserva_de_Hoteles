namespace HotelBooking.Application.Hotels.Queries.GetHotelById;

public sealed class HotelDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public List<RoomTypeDto> RoomTypes { get; init; } = [];
}

public sealed class RoomTypeDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int MaxGuests { get; init; }
    public decimal BasePrice { get; init; }
    public string Currency { get; init; } = string.Empty;
}
