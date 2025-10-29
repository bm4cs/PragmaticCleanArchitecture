using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Application.UnitTests.Apartments;

public class ApartmentData
{
    public static Apartment Create() =>
        new(
            Guid.NewGuid(),
            new Name("Test Apartment"),
            new Description("Test Description"),
            new Address("Country", "State", "ZipCode", "City", "Street"),
            new Money(100.0m, Currency.USD),
            Money.Zero(),
            []
        );
}
