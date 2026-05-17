using System.Text.RegularExpressions;
using HotelBooking.Domain.Abstractions;
using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Email> Create(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return DomainError.Validation(
                "Email.Empty",
                "Email cannot be empty");

        if (email.Length > 254)
            return DomainError.Validation(
                "Email.TooLong",
                "Email cannot exceed 254 characters");

        if (!EmailRegex.IsMatch(email))
            return DomainError.Validation(
                "Email.InvalidFormat",
                "Email format is invalid");

        return new Email(email.ToLowerInvariant());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
