using FluentValidation;

namespace HotelBooking.Application.RoomTypes.Commands.CreateRoomType;

public sealed class CreateRoomTypeCommandValidator : AbstractValidator<CreateRoomTypeCommand>
{
    public CreateRoomTypeCommandValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.MaxGuests)
            .GreaterThan(0)
            .LessThanOrEqualTo(10);

        RuleFor(x => x.BasePrice)
            .GreaterThan(0);

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Length(3);
    }
}
