using HotelBooking.Domain.Abstractions;
using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Hotels;

public sealed class Hotel : Entity<HotelId>, IAggregateRoot
{
    private readonly List<RoomType> _roomTypes = [];

    private Hotel(
        HotelId id,
        string name,
        string address,
        string city,
        string country) : base(id)
    {
        Name = name;
        Address = address;
        City = city;
        Country = country;
    }

    public string Name { get; private set; }
    public string Address { get; private set; }
    public string City { get; private set; }
    public string Country { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyList<RoomType> RoomTypes => _roomTypes.AsReadOnly();

    public static Result<Hotel> Create(
        string name,
        string address,
        string city,
        string country)
    {
        if (string.IsNullOrWhiteSpace(name))
            return DomainError.Validation(
                "Hotel.EmptyName",
                "Hotel name cannot be empty");

        if (string.IsNullOrWhiteSpace(address))
            return DomainError.Validation(
                "Hotel.EmptyAddress",
                "Hotel address cannot be empty");

        if (string.IsNullOrWhiteSpace(city))
            return DomainError.Validation(
                "Hotel.EmptyCity",
                "City cannot be empty");

        if (string.IsNullOrWhiteSpace(country))
            return DomainError.Validation(
                "Hotel.EmptyCountry",
                "Country cannot be empty");

        var hotel = new Hotel(
            HotelId.New(),
            name.Trim(),
            address.Trim(),
            city.Trim(),
            country.Trim())
        {
            CreatedAt = DateTime.UtcNow
        };

        return hotel;
    }

    public Result Update(string name, string address, string city, string country)
    {
        if (string.IsNullOrWhiteSpace(name))
            return DomainError.Validation(
                "Hotel.EmptyName",
                "Hotel name cannot be empty");

        if (string.IsNullOrWhiteSpace(address))
            return DomainError.Validation(
                "Hotel.EmptyAddress",
                "Hotel address cannot be empty");

        Name = name.Trim();
        Address = address.Trim();
        City = city.Trim();
        Country = country.Trim();
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result Deactivate()
    {
        if (!IsActive)
            return DomainError.Conflict(
                "Hotel.AlreadyInactive",
                "Hotel is already inactive");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }
}
