using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.RatePlans.Queries.GetRatePlanPrice;

public sealed record GetRatePlanPriceQuery(
    Guid RatePlanId,
    DateOnly CheckIn,
    DateOnly CheckOut) : IRequest<Result<RatePlanPriceDto>>;
