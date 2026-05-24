namespace HotelBooking.Application.Availability.Queries.GetAvailability;

public sealed class AvailabilityDto
{
    public Guid HotelId { get; init; }
    public string HotelName { get; init; } = string.Empty;
    public DateOnly CheckIn { get; init; }
    public DateOnly CheckOut { get; init; }
    public int Nights { get; init; }
    public List<AvailableRoomDto> AvailableRooms { get; init; } = [];
}

public sealed class AvailableRoomDto
{
    public Guid RoomTypeId { get; init; }
    public string RoomTypeName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int MaxGuests { get; init; }
    public decimal PricePerNight { get; init; }
    public decimal TotalPrice { get; init; }
    public string Currency { get; init; } = string.Empty;
    public int AvailableRooms { get; init; }
}
