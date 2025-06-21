using Bookify.Domain.Abstractions;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Apartments;

public sealed class Apartment(
    Guid id,
    Address address,
    Money cleaningFee,
    Description description,
    Name name,
    Money price,
    List<Amenity> amenities
) : Entity(id)
{
    public Address Address { get; private set; } = address;

    public List<Amenity> Amenities { get; private set; } = amenities;

    public Money CleaningFee { get; private set; } = cleaningFee;

    public Description Description { get; private set; } = description;

    public DateTime? LastBookedOnUtc { get; private set; }

    public Name Name { get; private set; } = name;

    public Money Price { get; private set; } = price;
}
