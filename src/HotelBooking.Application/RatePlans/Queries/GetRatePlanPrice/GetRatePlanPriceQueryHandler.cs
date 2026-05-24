using HotelBooking.Application.Abstractions;
using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.RatePlans.Queries.GetRatePlanPrice;

public sealed class GetRatePlanPriceQueryHandler
    : IRequestHandler<GetRatePlanPriceQuery, Result<RatePlanPriceDto>>
{
    private readonly IReadDbConnection _db;

    public GetRatePlanPriceQueryHandler(IReadDbConnection db)
    {
        _db = db;
    }

    public async Task<Result<RatePlanPriceDto>> Handle(
        GetRatePlanPriceQuery request,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                rp.Id AS RatePlanId,
                rp.Name,
                rp.PricePerNight,
                rp.PricePerNight * DATEDIFF(day, @CheckIn, @CheckOut) AS TotalPrice,
                rp.Currency,
                DATEDIFF(day, @CheckIn, @CheckOut) AS Nights,
                rp.ValidFrom,
                rp.ValidTo
            FROM RatePlans rp
            WHERE rp.Id = @RatePlanId
                AND rp.IsActive = 1
                AND rp.ValidFrom <= @CheckIn
                AND rp.ValidTo >= @CheckOut
            """;

        var result = await _db.QueryFirstOrDefaultAsync<RatePlanPriceDto>(
            sql,
            new
            {
                request.RatePlanId,
                request.CheckIn,
                request.CheckOut
            },
            cancellationToken: cancellationToken);

        if (result is null)
            return DomainError.NotFound(
                "RatePlan.NotFound",
                "No active rate plan found for the requested dates");

        return result;
    }
}
