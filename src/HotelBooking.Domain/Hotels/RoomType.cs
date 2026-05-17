using HotelBooking.Domain.Abstractions;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.ValueObjects;

namespace HotelBooking.Domain.Hotels;

public sealed class RoomType : Entity<RoomTypeId>
{
    private RoomType(
        RoomTypeId id,
        HotelId hotelId,
        string name,
        string description,
        int maxGuests,
        Money basePrice) : base(id)
    {
        HotelId = hotelId;
        Name = name;
        Description = description;
        MaxGuests = maxGuests;
        BasePrice = basePrice;
    }

    public HotelId HotelId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int MaxGuests { get; private set; }
    public Money BasePrice { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public static Result<RoomType> Create(
        HotelId hotelId,
        string name,
        string description,
        int maxGuests,
        Money basePrice)
    {
        if (string.IsNullOrWhiteSpace(name))
            return DomainError.Validation(
                "RoomType.EmptyName",
                "Room type name cannot be empty");

        if (maxGuests <= 0)
            return DomainError.Validation(
                "RoomType.InvalidCapacity",
                "Max guests must be greater than zero");

        if (basePrice.Amount <= 0)
            return DomainError.Validation(
                "RoomType.InvalidPrice",
                "Base price must be greater than zero");

        return new RoomType(
            RoomTypeId.New(),
            hotelId,
            name.Trim(),
            description.Trim(),
            maxGuests,
            basePrice)
        {
            CreatedAt = DateTime.UtcNow
        };
    }

    public Result Update(string name, string description, int maxGuests, Money basePrice)
    {
        if (string.IsNullOrWhiteSpace(name))
            return DomainError.Validation(
                "RoomType.EmptyName",
                "Room type name cannot be empty");

        if (maxGuests <= 0)
            return DomainError.Validation(
                "RoomType.InvalidCapacity",
                "Max guests must be greater than zero");

        if (basePrice.Amount <= 0)
            return DomainError.Validation(
                "RoomType.InvalidPrice",
                "Base price must be greater than zero");

        Name = name.Trim();
        Description = description.Trim();
        MaxGuests = maxGuests;
        BasePrice = basePrice;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result Deactivate()
    {
        if (!IsActive)
            return DomainError.Conflict(
                "RoomType.AlreadyInactive",
                "Room type is already inactive");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }
}
