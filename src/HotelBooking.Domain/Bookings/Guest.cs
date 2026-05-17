using HotelBooking.Domain.Abstractions;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.ValueObjects;

namespace HotelBooking.Domain.Bookings;

public sealed class Guest : Entity<GuestId>
{
    private Guest(
        GuestId id,
        string firstName,
        string lastName,
        Email email,
        string phone) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; private set; }
    public string Phone { get; private set; }
    public string FullName => $"{FirstName} {LastName}";

    public static Result<Guest> Create(
        string firstName,
        string lastName,
        string email,
        string phone)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return DomainError.Validation(
                "Guest.EmptyFirstName",
                "First name cannot be empty");

        if (string.IsNullOrWhiteSpace(lastName))
            return DomainError.Validation(
                "Guest.EmptyLastName",
                "Last name cannot be empty");

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
            return emailResult.Error;

        if (string.IsNullOrWhiteSpace(phone))
            return DomainError.Validation(
                "Guest.EmptyPhone",
                "Phone cannot be empty");

        return new Guest(
            GuestId.New(),
            firstName.Trim(),
            lastName.Trim(),
            emailResult.Value,
            phone.Trim());
    }
}
