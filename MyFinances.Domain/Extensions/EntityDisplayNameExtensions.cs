using System.ComponentModel;

namespace MyFinances.Domain.Extensions;

public static class EntityDisplayNameExtensions
{
    public static string GetDisplayName(this Type type)
    {
        var displayNameAttribute = type.GetCustomAttributes(typeof(DisplayNameAttribute), true)
            .FirstOrDefault() as DisplayNameAttribute;

        return displayNameAttribute?.DisplayName ?? type.Name;
    }
}