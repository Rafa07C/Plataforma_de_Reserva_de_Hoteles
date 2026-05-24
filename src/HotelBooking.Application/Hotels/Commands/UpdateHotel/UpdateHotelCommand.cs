using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.Hotels.Commands.UpdateHotel;

public sealed record UpdateHotelCommand(
    Guid HotelId,
    string Name,
    string Address,
    string City,
    string Country) : IRequest<Result>;
