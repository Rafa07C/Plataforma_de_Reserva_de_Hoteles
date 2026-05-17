namespace HotelBooking.Domain.Common;

public sealed record DomainError(string Code, string Message, ErrorType Type)
{
    public static readonly DomainError None = new(
        string.Empty,
        string.Empty,
        ErrorType.Unexpected);

    public static readonly DomainError NullValue = new(
        "General.NullValue",
        "A null value was provided",
        ErrorType.Unexpected);

    public static DomainError Validation(string code, string message) =>
        new(code, message, ErrorType.Validation);

    public static DomainError NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);

    public static DomainError Conflict(string code, string message) =>
        new(code, message, ErrorType.Conflict);

    public static DomainError Unprocessable(string code, string message) =>
        new(code, message, ErrorType.Unprocessable);

    public static DomainError Unauthorized(string code, string message) =>
        new(code, message, ErrorType.Unauthorized);
}
