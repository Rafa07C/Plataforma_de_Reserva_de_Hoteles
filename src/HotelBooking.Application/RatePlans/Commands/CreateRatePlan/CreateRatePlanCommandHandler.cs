using HotelBooking.Application.Abstractions;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Hotels;
using HotelBooking.Domain.ValueObjects;
using MediatR;

namespace HotelBooking.Application.RatePlans.Commands.CreateRatePlan;

public sealed class CreateRatePlanCommandHandler
    : IRequestHandler<CreateRatePlanCommand, Result<Guid>>
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRatePlanCommandHandler(
        IRoomTypeRepository roomTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _roomTypeRepository = roomTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateRatePlanCommand request,
        CancellationToken cancellationToken)
    {
        var roomType = await _roomTypeRepository.GetByIdAsync(
            new RoomTypeId(request.RoomTypeId),
            cancellationToken);

        if (roomType is null)
            return DomainError.NotFound(
                "RoomType.NotFound",
                "Room type not found");

        var moneyResult = Money.Create(request.PricePerNight, request.Currency);
        if (moneyResult.IsFailure)
            return moneyResult.Error;

        var ratePlanResult = RatePlan.Create(
            new RoomTypeId(request.RoomTypeId),
            request.Name,
            moneyResult.Value,
            request.ValidFrom,
            request.ValidTo);

        if (ratePlanResult.IsFailure)
            return ratePlanResult.Error;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ratePlanResult.Value.Id.Value;
    }
}
