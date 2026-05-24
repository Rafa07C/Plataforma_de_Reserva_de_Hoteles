using HotelBooking.Application.Abstractions;
using HotelBooking.Domain.Bookings;
using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.Bookings.Commands.ConfirmBooking;

public sealed class ConfirmBookingCommandHandler
    : IRequestHandler<ConfirmBookingCommand, Result>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmBookingCommandHandler(
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        ConfirmBookingCommand request,
        CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(
            new BookingId(request.BookingId),
            cancellationToken);

        if (booking is null)
            return DomainError.NotFound(
                "Booking.NotFound",
                "Booking not found");

        var result = booking.Confirm();
        if (result.IsFailure)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
