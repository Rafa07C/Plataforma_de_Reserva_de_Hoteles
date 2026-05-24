using HotelBooking.Application.Abstractions;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Hotels;
using MediatR;

namespace HotelBooking.Application.Hotels.Commands.UpdateHotel;

public sealed class UpdateHotelCommandHandler
    : IRequestHandler<UpdateHotelCommand, Result>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateHotelCommandHandler(
        IHotelRepository hotelRepository,
        IUnitOfWork unitOfWork)
    {
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateHotelCommand request,
        CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(
            new HotelId(request.HotelId),
            cancellationToken);

        if (hotel is null)
            return DomainError.NotFound(
                "Hotel.NotFound",
                "Hotel not found");

        var updateResult = hotel.Update(
            request.Name,
            request.Address,
            request.City,
            request.Country);

        if (updateResult.IsFailure)
            return updateResult;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
