namespace HotelBooking.Application.Common;

public sealed class PagedResult<T>
{
    internal PagedResult(
        IReadOnlyList<T> data,
        int pageNumber,
        int pageSize,
        int totalRecords)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
    }

    public IReadOnlyList<T> Data { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalRecords { get; }
    public int TotalPages { get; }
}

public static class PagedResult
{
    public static PagedResult<T> Create<T>(
        IReadOnlyList<T> data,
        int pageNumber,
        int pageSize,
        int totalRecords) => new(data, pageNumber, pageSize, totalRecords);
}
