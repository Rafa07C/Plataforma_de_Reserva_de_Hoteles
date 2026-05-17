using HotelBooking.Domain.Abstractions;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.ValueObjects;

namespace HotelBooking.Domain.Hotels;

public sealed class RatePlan : Entity<RatePlanId>
{
    private RatePlan(
        RatePlanId id,
        RoomTypeId roomTypeId,
        string name,
        Money pricePerNight,
        DateOnly validFrom,
        DateOnly validTo) : base(id)
    {
        RoomTypeId = roomTypeId;
        Name = name;
        PricePerNight = pricePerNight;
        ValidFrom = validFrom;
        ValidTo = validTo;
    }

    public RoomTypeId RoomTypeId { get; private set; }
    public string Name { get; private set; }
    public Money PricePerNight { get; private set; }
    public DateOnly ValidFrom { get; private set; }
    public DateOnly ValidTo { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; }

    public static Result<RatePlan> Create(
        RoomTypeId roomTypeId,
        string name,
        Money pricePerNight,
        DateOnly validFrom,
        DateOnly validTo)
    {
        if (string.IsNullOrWhiteSpace(name))
            return DomainError.Validation(
                "RatePlan.EmptyName",
                "Rate plan name cannot be empty");

        if (pricePerNight.Amount <= 0)
            return DomainError.Validation(
                "RatePlan.InvalidPrice",
                "Price per night must be greater than zero");

        if (validFrom >= validTo)
            return DomainError.Validation(
                "RatePlan.InvalidDateRange",
                "Valid from date must be before valid to date");

        return new RatePlan(
            RatePlanId.New(),
            roomTypeId,
            name.Trim(),
            pricePerNight,
            validFrom,
            validTo)
        {
            CreatedAt = DateTime.UtcNow
        };
    }

    public Result<Money> CalculatePrice(DateRange dateRange)
    {
        if (!IsActive)
            return DomainError.Conflict(
                "RatePlan.Inactive",
                "Rate plan is not active");

        if (dateRange.CheckIn < ValidFrom || dateRange.CheckOut > ValidTo)
            return DomainError.Validation(
                "RatePlan.OutOfRange",
                "Date range is outside rate plan validity period");

        return PricePerNight.Multiply(dateRange.Nights);
    }

    public Result Deactivate()
    {
        if (!IsActive)
            return DomainError.Conflict(
                "RatePlan.AlreadyInactive",
                "Rate plan is already inactive");

        IsActive = false;
        return Result.Success();
    }
}
