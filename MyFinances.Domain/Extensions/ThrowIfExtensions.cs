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
            throw new ArgumentOutOfRangeException(paramName,
                $"The value '{paramName}' must be between {min} and {max}.");
    }

    public static void ThrowIfOutOfRange(this decimal value, decimal min, decimal max, string paramName)
    {
        if (value < min || value > max)
            throw new ArgumentOutOfRangeException(paramName,
                $"The value '{paramName}' must be between {min} and {max}.");
    }

    public static void ThrowIfLessThanOrEqualTo(this decimal value, decimal threshold, string paramName)
    {
        if (value <= threshold)
            throw new ArgumentOutOfRangeException(paramName,
                $"The value '{paramName}' must be greater than {threshold}.");
    }

    public static void ThrowIfLessThanOrEqualTo(this int value, int threshold, string paramName)
    {
        if (value <= threshold)
            throw new ArgumentOutOfRangeException(paramName,
                $"The value '{paramName}' must be greater than {threshold}.");
    }

    public static void ThrowIfNotInEnum<T>(this int value, string paramName) where T : System.Enum
    {
        if (!System.Enum.IsDefined(typeof(T), value))
            throw new ArgumentOutOfRangeException(paramName,
                $"The value '{value}' is not valid for enum type '{typeof(T).Name}'.");
    }

    public static void ThrowIfDefault<T>(this T value, string paramName) where T : struct
    {
        if (EqualityComparer<T>.Default.Equals(value, default))
            throw new ArgumentException($"The argument '{paramName}' cannot be the default value.", paramName);
    }
}