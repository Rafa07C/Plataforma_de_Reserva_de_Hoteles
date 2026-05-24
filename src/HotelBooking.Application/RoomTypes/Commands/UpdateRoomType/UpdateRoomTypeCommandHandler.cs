using HotelBooking.Application.Abstractions;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Hotels;
using HotelBooking.Domain.ValueObjects;
using MediatR;

namespace HotelBooking.Application.RoomTypes.Commands.UpdateRoomType;

public sealed class UpdateRoomTypeCommandHandler
    : IRequestHandler<UpdateRoomTypeCommand, Result>
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoomTypeCommandHandler(
        IRoomTypeRepository roomTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _roomTypeRepository = roomTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateRoomTypeCommand request,
        CancellationToken cancellationToken)
    {
        var roomType = await _roomTypeRepository.GetByIdAsync(
            new RoomTypeId(request.RoomTypeId),
            cancellationToken);

        if (roomType is null)
            return DomainError.NotFound(
                "RoomType.NotFound",
                "Room type not found");

        var moneyResult = Money.Create(request.BasePrice, request.Currency);
        if (moneyResult.IsFailure)
            return moneyResult.Error;

        var updateResult = roomType.Update(
            request.Name,
            request.Description,
            request.MaxGuests,
            moneyResult.Value);

        if (updateResult.IsFailure)
            return updateResult;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
