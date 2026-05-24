using FluentValidation;

namespace HotelBooking.Application.Hotels.Commands.CreateHotel;

public sealed class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(100);
    }
}
