using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings;

public sealed class PricingService
{
    public PricingDetails CalculatePrice(Apartment apartment, DateRange period)
    {
        var currency = apartment.Price.Currency;

        var priceForPeriod = new Money(apartment.Price.Amount * period.LengthInDays, currency);

        decimal percentageUpCharge = 0;
        foreach (var amenity in apartment.Amenities)
        {
            percentageUpCharge += amenity switch
            {
                Amenity.GardenView or Amenity.MountainView => 0.05m, // 5% upcharge
                Amenity.AirConditioning => 0.01m, // 1% upcharge
                Amenity.Parking => 0.01m, // 1% upcharge
                _ => 0, // No upcharge for other amenities
            };
        }

        var amenitiesUpcharge = Money.Zero(currency);
        if (percentageUpCharge > 0)
        {
            amenitiesUpcharge = new Money(priceForPeriod.Amount * percentageUpCharge, currency);
        }

        var totalPrice = Money.Zero(currency);
        totalPrice += priceForPeriod;
        if (!apartment.CleaningFee.IsZero())
        {
            totalPrice += apartment.CleaningFee;
        }

        totalPrice += amenitiesUpcharge;

        return new PricingDetails(
            priceForPeriod,
            apartment.CleaningFee,
            amenitiesUpcharge,
            totalPrice
        );
    }
}
