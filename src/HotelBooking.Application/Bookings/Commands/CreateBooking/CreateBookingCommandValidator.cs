using FluentValidation;

namespace HotelBooking.Application.Bookings.Commands.CreateBooking;

public sealed class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty();

        RuleFor(x => x.RoomTypeId)
            .NotEmpty();

        RuleFor(x => x.RatePlanId)
            .NotEmpty();

        RuleFor(x => x.CheckIn)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow.Date))
            .WithMessage("Check-in date cannot be in the past");

        RuleFor(x => x.CheckOut)
            .NotEmpty()
            .GreaterThan(x => x.CheckIn)
            .WithMessage("Check-out date must be after check-in date");

        RuleFor(x => x.GuestsCount)
            .GreaterThan(0)
            .LessThanOrEqualTo(10);

        RuleFor(x => x.GuestFirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.GuestLastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.GuestEmail)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(254);

        RuleFor(x => x.GuestPhone)
            .NotEmpty()
            .MaximumLength(20);
    }
}
