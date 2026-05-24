using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.RatePlans.Commands.CreateRatePlan;

public sealed record CreateRatePlanCommand(
    Guid RoomTypeId,
    string Name,
    decimal PricePerNight,
    string Currency,
    DateOnly ValidFrom,
    DateOnly ValidTo) : IRequest<Result<Guid>>;
