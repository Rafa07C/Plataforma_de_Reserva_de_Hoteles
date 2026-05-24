using HotelBooking.Application.Abstractions;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Hotels;
using HotelBooking.Domain.ValueObjects;
using MediatR;

namespace HotelBooking.Application.RoomTypes.Commands.CreateRoomType;

public sealed class CreateRoomTypeCommandHandler
    : IRequestHandler<CreateRoomTypeCommand, Result<Guid>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoomTypeCommandHandler(
        IHotelRepository hotelRepository,
        IUnitOfWork unitOfWork)
    {
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateRoomTypeCommand request,
        CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(
            new HotelId(request.HotelId),
            cancellationToken);

        if (hotel is null)
            return DomainError.NotFound(
                "Hotel.NotFound",
                "Hotel not found");

        var moneyResult = Money.Create(request.BasePrice, request.Currency);
        if (moneyResult.IsFailure)
            return moneyResult.Error;

        var roomTypeResult = RoomType.Create(
            new HotelId(request.HotelId),
            request.Name,
            request.Description,
            request.MaxGuests,
            moneyResult.Value);

        if (roomTypeResult.IsFailure)
            return roomTypeResult.Error;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return roomTypeResult.Value.Id.Value;
    }
}
