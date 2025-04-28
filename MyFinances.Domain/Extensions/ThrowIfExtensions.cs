namespace MyFinances.Domain.Extensions;

public static class ThrowIfExtensions
{
    public static void ThrowIfNull(this object? value, string paramName)
    {
        if (value is null)
            throw new ArgumentNullException(paramName, $"The argument '{paramName}' cannot be null.");
    }

    public static void ThrowIfNullOrEmpty(this string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"The argument '{paramName}' cannot be null or empty.", paramName);
    }

    public static void ThrowIfOutOfRange(this int value, int min, int max, string paramName)
    {
        if (value < min || value > max)
            throw new ArgumentOutOfRangeException(paramName, $"The value '{paramName}' must be between {min} and {max}.");
    }
}