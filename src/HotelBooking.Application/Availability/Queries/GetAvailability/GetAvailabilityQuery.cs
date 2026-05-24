using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.Availability.Queries.GetAvailability;

public sealed record GetAvailabilityQuery(
    Guid HotelId,
    DateOnly CheckIn,
    DateOnly CheckOut,
    int GuestsCount) : IRequest<Result<AvailabilityDto>>;
