using HotelBooking.Domain.Abstractions;
using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.ValueObjects;

public sealed class Money : ValueObject
{
    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; }
    public string Currency { get; }

    public static Result<Money> Create(decimal amount, string currency)
    {
        if (amount < 0)
            return DomainError.Validation(
                "Money.NegativeAmount",
                "Amount cannot be negative");

        if (string.IsNullOrWhiteSpace(currency))
            return DomainError.Validation(
                "Money.EmptyCurrency",
                "Currency cannot be empty");

        if (currency.Length != 3)
            return DomainError.Validation(
                "Money.InvalidCurrency",
                "Currency must be a 3-letter ISO code (e.g. USD, HNL)");

        return new Money(amount, currency.ToUpperInvariant());
    }

    public static Money Zero(string currency) => new(0, currency.ToUpperInvariant());

    public Result<Money> Add(Money other)
    {
        if (Currency != other.Currency)
            return DomainError.Validation(
                "Money.CurrencyMismatch",
                $"Cannot add amounts in different currencies: {Currency} and {other.Currency}");

        return new Money(Amount + other.Amount, Currency);
    }

    public Result<Money> Multiply(decimal factor)
    {
        if (factor < 0)
            return DomainError.Validation(
                "Money.NegativeFactor",
                "Multiplication factor cannot be negative");

        return new Money(Amount * factor, Currency);
    }

    public bool IsGreaterThan(Money other) =>
        Currency == other.Currency && Amount > other.Amount;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
