using HotelBooking.Domain.Abstractions;
using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Hotels;

public sealed class RoomInventory : Entity<RoomInventoryId>
{
    private RoomInventory(
        RoomInventoryId id,
        RoomTypeId roomTypeId,
        DateOnly date,
        int totalRooms,
        int availableRooms) : base(id)
    {
        RoomTypeId = roomTypeId;
        Date = date;
        TotalRooms = totalRooms;
        AvailableRooms = availableRooms;
    }

    public RoomTypeId RoomTypeId { get; private set; }
    public DateOnly Date { get; private set; }
    public int TotalRooms { get; private set; }
    public int AvailableRooms { get; private set; }
    public byte[] RowVersion { get; private set; } = [];

    public static Result<RoomInventory> Create(
        RoomTypeId roomTypeId,
        DateOnly date,
        int totalRooms)
    {
        if (totalRooms <= 0)
            return DomainError.Validation(
                "RoomInventory.InvalidTotalRooms",
                "Total rooms must be greater than zero");

        if (date < DateOnly.FromDateTime(DateTime.UtcNow.Date))
            return DomainError.Validation(
                "RoomInventory.PastDate",
                "Cannot create inventory for past dates");

        return new RoomInventory(
            RoomInventoryId.New(),
            roomTypeId,
            date,
            totalRooms,
            totalRooms);
    }

    public Result Reserve()
    {
        if (AvailableRooms <= 0)
            return DomainError.Conflict(
                "RoomInventory.NoAvailability",
                "No rooms available for this date");

        AvailableRooms--;
        return Result.Success();
    }

    public Result Release()
    {
        if (AvailableRooms >= TotalRooms)
            return DomainError.Conflict(
                "RoomInventory.AlreadyFull",
                "Cannot release more rooms than total capacity");

        AvailableRooms++;
        return Result.Success();
    }
}
