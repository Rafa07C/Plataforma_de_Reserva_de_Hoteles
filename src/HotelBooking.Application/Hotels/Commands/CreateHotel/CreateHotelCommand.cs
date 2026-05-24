using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.Hotels.Commands.CreateHotel;

public sealed record CreateHotelCommand(
    string Name,
    string Address,
    string City,
    string Country) : IRequest<Result<Guid>>;
