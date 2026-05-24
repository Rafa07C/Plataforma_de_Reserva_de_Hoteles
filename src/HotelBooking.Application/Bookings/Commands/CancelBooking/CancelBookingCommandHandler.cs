using HotelBooking.Application.Abstractions;
using HotelBooking.Domain.Bookings;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Hotels;
using MediatR;

namespace HotelBooking.Application.Bookings.Commands.CancelBooking;

public sealed class CancelBookingCommandHandler
    : IRequestHandler<CancelBookingCommand, Result>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomInventoryRepository _inventoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelBookingCommandHandler(
        IBookingRepository bookingRepository,
        IRoomInventoryRepository inventoryRepository,
        IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _inventoryRepository = inventoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        CancelBookingCommand request,
        CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(
            new BookingId(request.BookingId),
            cancellationToken);

        if (booking is null)
            return DomainError.NotFound(
                "Booking.NotFound",
                "Booking not found");

        var cancelResult = booking.Cancel(request.Reason);
        if (cancelResult.IsFailure)
            return cancelResult;

        var inventories = await _inventoryRepository.GetByRoomTypeAndDateRangeAsync(
            booking.RoomTypeId,
            booking.DateRange.CheckIn,
            booking.DateRange.CheckOut,
            cancellationToken);

        foreach (var inventory in inventories)
        {
            inventory.Release();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
