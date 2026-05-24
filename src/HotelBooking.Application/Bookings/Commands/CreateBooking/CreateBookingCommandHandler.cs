using HotelBooking.Application.Abstractions;
using HotelBooking.Domain.Bookings;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Hotels;
using HotelBooking.Domain.ValueObjects;
using MediatR;

namespace HotelBooking.Application.Bookings.Commands.CreateBooking;

public sealed class CreateBookingCommandHandler
    : IRequestHandler<CreateBookingCommand, Result<Guid>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IRoomInventoryRepository _inventoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookingCommandHandler(
        IBookingRepository bookingRepository,
        IHotelRepository hotelRepository,
        IRoomInventoryRepository inventoryRepository,
        IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _hotelRepository = hotelRepository;
        _inventoryRepository = inventoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateBookingCommand request,
        CancellationToken cancellationToken)
    {
        var hotelId = new HotelId(request.HotelId);
        var roomTypeId = new RoomTypeId(request.RoomTypeId);
        var ratePlanId = new RatePlanId(request.RatePlanId);

        var hotel = await _hotelRepository.GetByIdAsync(hotelId, cancellationToken);
        if (hotel is null)
            return DomainError.NotFound("Hotel.NotFound", "Hotel not found");

        var dateRangeResult = DateRange.Create(request.CheckIn, request.CheckOut);
        if (dateRangeResult.IsFailure)
            return dateRangeResult.Error;

        var guestsCountResult = GuestsCount.Create(request.GuestsCount);
        if (guestsCountResult.IsFailure)
            return guestsCountResult.Error;

        var hasAvailability = await _inventoryRepository.HasAvailabilityAsync(
            roomTypeId,
            request.CheckIn,
            request.CheckOut,
            cancellationToken);

        if (!hasAvailability)
            return DomainError.Conflict(
                "Booking.NoAvailability",
                "No rooms available for the selected dates");

        var ratePlan = hotel.RoomTypes
            .FirstOrDefault(rt => rt.Id == roomTypeId)
            ?.Id;

        if (ratePlan is null)
            return DomainError.NotFound(
                "RoomType.NotFound",
                "Room type not found in this hotel");

        var priceResult = Money.Create(0, "USD");
        if (priceResult.IsFailure)
            return priceResult.Error;

        var bookingResult = Booking.Create(
            hotelId,
            roomTypeId,
            ratePlanId,
            dateRangeResult.Value,
            guestsCountResult.Value,
            priceResult.Value);

        if (bookingResult.IsFailure)
            return bookingResult.Error;

        var booking = bookingResult.Value;

        var guestResult = Guest.Create(
            request.GuestFirstName,
            request.GuestLastName,
            request.GuestEmail,
            request.GuestPhone);

        if (guestResult.IsFailure)
            return guestResult.Error;

        var addGuestResult = booking.AddGuest(guestResult.Value);
        if (addGuestResult.IsFailure)
            return addGuestResult.Error;

        var inventories = await _inventoryRepository.GetByRoomTypeAndDateRangeAsync(
            roomTypeId,
            request.CheckIn,
            request.CheckOut,
            cancellationToken);

        foreach (var inventory in inventories)
        {
            var reserveResult = inventory.Reserve();
            if (reserveResult.IsFailure)
                return reserveResult.Error;
        }

        await _bookingRepository.AddAsync(booking, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return booking.Id.Value;
    }
}
