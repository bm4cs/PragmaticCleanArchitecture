using Bookify.Domain.Bookings;
using Bookify.Domain.Shared;
using Bookify.Domain.UnitTests.Apartments;
using FluentAssertions;

namespace Bookify.Domain.UnitTests.Bookings;

public class PricingServiceTests
{
    [Fact]
    public void CalculatePrice_Should_ReturnCorrectTotalPrice()
    {
        // Arrange
        var price = new Money(10.0m, Currency.USD);
        var period = DateRange.Create(new DateOnly(2026, 1, 10), new DateOnly(2026, 1, 16));
        var expectedTotalPrice = new Money(price.Amount * period.LengthInDays, price.Currency);
        var apartment = ApartmentData.Create(price);
        var pricingService = new PricingService();

        // Act
        var pricingDetails = pricingService.CalculatePrice(apartment, period);

        // Assert
        pricingDetails.TotalPrice.Should().Be(expectedTotalPrice);
    }

    [Fact]
    public void CalculatePrice_Should_ReturnCorrectTotalPrice_WhenCleaningFeeIsIncluded()
    {
        // Arrange
        var price = new Money(10.0m, Currency.USD);
        var period = DateRange.Create(new DateOnly(2026, 1, 10), new DateOnly(2026, 1, 16));
        var cleaningFee = new Money(99.99m, Currency.USD);
        var expectedTotalPrice = new Money(
            price.Amount * period.LengthInDays + cleaningFee.Amount,
            price.Currency
        );
        var apartment = ApartmentData.Create(price, cleaningFee);
        var pricingService = new PricingService();

        // Act
        var pricingDetails = pricingService.CalculatePrice(apartment, period);

        // Assert
        pricingDetails.TotalPrice.Should().Be(expectedTotalPrice);
    }
}
