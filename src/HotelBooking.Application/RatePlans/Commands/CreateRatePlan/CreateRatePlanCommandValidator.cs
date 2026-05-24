using FluentValidation;

namespace HotelBooking.Application.RatePlans.Commands.CreateRatePlan;

public sealed class CreateRatePlanCommandValidator : AbstractValidator<CreateRatePlanCommand>
{
    public CreateRatePlanCommandValidator()
    {
        RuleFor(x => x.RoomTypeId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.PricePerNight)
            .GreaterThan(0);

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Length(3);

        RuleFor(x => x.ValidFrom)
            .NotEmpty();

        RuleFor(x => x.ValidTo)
            .NotEmpty()
            .GreaterThan(x => x.ValidFrom)
            .WithMessage("Valid to date must be after valid from date");
    }
}
