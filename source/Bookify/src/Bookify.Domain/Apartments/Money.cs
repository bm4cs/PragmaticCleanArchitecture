namespace Bookify.Domain.Apartments;

public record Money(decimal Amount, Currency Currency)
{
    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new InvalidOperationException("Cannot add Money with different currencies.");
        }

        return left with
        {
            Amount = left.Amount + right.Amount,
        };
    }

    public static Money Zero() => new(0, Currency.None);
}
