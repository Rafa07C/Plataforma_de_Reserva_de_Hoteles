using HotelBooking.Domain.Abstractions;
using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.ValueObjects;

public sealed class DateRange : ValueObject
{
    private DateRange(DateOnly checkIn, DateOnly checkOut)
    {
        CheckIn = checkIn;
        CheckOut = checkOut;
    }

    public DateOnly CheckIn { get; }
    public DateOnly CheckOut { get; }

    public int Nights => CheckOut.DayNumber - CheckIn.DayNumber;

    public static Result<DateRange> Create(DateOnly checkIn, DateOnly checkOut, int maxNights = 30)
    {
        if (checkIn >= checkOut)
            return DomainError.Validation(
                "DateRange.InvalidRange",
                "Check-in date must be before check-out date");

        if (checkIn < DateOnly.FromDateTime(DateTime.UtcNow.Date))
            return DomainError.Validation(
                "DateRange.PastDate",
                "Check-in date cannot be in the past");

        if (checkOut.DayNumber - checkIn.DayNumber > maxNights)
            return DomainError.Validation(
                "DateRange.ExceedsMaxNights",
                $"Stay cannot exceed {maxNights} nights");

        return new DateRange(checkIn, checkOut);
    }

    public bool Overlaps(DateRange other) =>
        CheckIn < other.CheckOut && CheckOut > other.CheckIn;

    public IEnumerable<DateOnly> EachNight()
    {
        var current = CheckIn;
        while (current < CheckOut)
        {
            yield return current;
            current = current.AddDays(1);
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CheckIn;
        yield return CheckOut;
    }
}
