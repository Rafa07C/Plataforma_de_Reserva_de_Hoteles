using HotelBooking.Application.Abstractions;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Hotels;
using MediatR;

namespace HotelBooking.Application.Hotels.Commands.DeleteHotel;

public sealed class DeleteHotelCommandHandler
    : IRequestHandler<DeleteHotelCommand, Result>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteHotelCommandHandler(
        IHotelRepository hotelRepository,
        IUnitOfWork unitOfWork)
    {
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteHotelCommand request,
        CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(
            new HotelId(request.HotelId),
            cancellationToken);

        if (hotel is null)
            return DomainError.NotFound(
                "Hotel.NotFound",
                "Hotel not found");

        var deactivateResult = hotel.Deactivate();
        if (deactivateResult.IsFailure)
            return deactivateResult;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
