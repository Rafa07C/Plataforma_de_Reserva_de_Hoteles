using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.Hotels.Queries.GetHotelById;

public sealed record GetHotelByIdQuery(Guid HotelId) : IRequest<Result<HotelDto>>;
