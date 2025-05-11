using System.ComponentModel.DataAnnotations.Schema;
using MyFinances.Domain.Extensions;

namespace MyFinances.Domain.ValueObjects;

[ComplexType]
public record Period
{
    private Period()
    {
    }

    public int Year { get; init; }
    public int Month { get; init; }

    public static Period Create(int year, int month)
    {
        month.ThrowIfOutOfRange(1, 12, nameof(month));

        return new Period
        {
            Year = year,
            Month = month
        };
    }

    public static Period Create(DateTime date)
    {
        return new Period
        {
            Year = date.Year,
            Month = date.Month
        };
    }

    public override string ToString()
    {
        return $"{Year:D4}-{Month:D2}";
    }
}