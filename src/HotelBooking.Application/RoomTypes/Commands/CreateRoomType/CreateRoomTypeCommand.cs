using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.RoomTypes.Commands.CreateRoomType;

public sealed record CreateRoomTypeCommand(
    Guid HotelId,
    string Name,
    string Description,
    int MaxGuests,
    decimal BasePrice,
    string Currency) : IRequest<Result<Guid>>;
