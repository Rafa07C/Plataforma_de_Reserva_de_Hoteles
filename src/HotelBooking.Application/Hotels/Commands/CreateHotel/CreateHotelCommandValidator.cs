using HotelBooking.Application.Abstractions;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Hotels;
using MediatR;

namespace HotelBooking.Application.Hotels.Commands.CreateHotel;

public sealed class CreateHotelCommandHandler
    : IRequestHandler<CreateHotelCommand, Result<Guid>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHotelCommandHandler(
        IHotelRepository hotelRepository,
        IUnitOfWork unitOfWork)
    {
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateHotelCommand request,
        CancellationToken cancellationToken)
    {
        var hotelResult = Hotel.Create(
            request.Name,
            request.Address,
            request.City,
            request.Country);

        if (hotelResult.IsFailure)
            return hotelResult.Error;

        await _hotelRepository.AddAsync(hotelResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return hotelResult.Value.Id.Value;
    }
}
