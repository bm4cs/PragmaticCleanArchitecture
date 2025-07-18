﻿namespace Bookify.Domain.Shared;

public record Currency
{
    internal static readonly Currency None = new("");
    public static readonly Currency USD = new("USD");
    public static readonly Currency EUR = new("EUR");
    public static readonly Currency GBP = new("GBP");
    public static readonly Currency JPY = new("JPY");
    public static readonly Currency AUD = new("AUD");

    private Currency(string code) => Code = code;

    public string Code { get; init; }

    public static Currency FromCode(string code)
    {
        return All.FirstOrDefault(c => c.Code == code)
            ?? throw new ArgumentException($"Unsupported currency code: {code}", nameof(code));
    }

    public static readonly IReadOnlyCollection<Currency> All = [USD, EUR, GBP, JPY, AUD];
}
