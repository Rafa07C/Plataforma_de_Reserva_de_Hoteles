using FluentValidation;

namespace HotelBooking.Application.Hotels.Commands.UpdateHotel;

public sealed class UpdateHotelCommandValidator : AbstractValidator<UpdateHotelCommand>
{
    public UpdateHotelCommandValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty();

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
