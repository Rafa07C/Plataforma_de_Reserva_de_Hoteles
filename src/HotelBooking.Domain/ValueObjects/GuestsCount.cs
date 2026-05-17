using HotelBooking.Domain.Abstractions;
using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.ValueObjects;

public sealed class GuestsCount : ValueObject
{
    private GuestsCount(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<GuestsCount> Create(int value, int maxGuests = 10)
    {
        if (value <= 0)
            return DomainError.Validation(
                "GuestsCount.InvalidValue",
                "Guests count must be greater than zero");

        if (value > maxGuests)
            return DomainError.Validation(
                "GuestsCount.ExceedsCapacity",
                $"Guests count cannot exceed {maxGuests}");

        return new GuestsCount(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
