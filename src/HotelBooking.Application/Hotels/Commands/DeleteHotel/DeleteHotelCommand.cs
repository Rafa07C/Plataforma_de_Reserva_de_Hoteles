using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.Hotels.Commands.DeleteHotel;

public sealed record DeleteHotelCommand(Guid HotelId) : IRequest<Result>;
