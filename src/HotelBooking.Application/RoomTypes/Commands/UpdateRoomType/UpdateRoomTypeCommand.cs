using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.RoomTypes.Commands.UpdateRoomType;

public sealed record UpdateRoomTypeCommand(
    Guid RoomTypeId,
    string Name,
    string Description,
    int MaxGuests,
    decimal BasePrice,
    string Currency) : IRequest<Result>;
