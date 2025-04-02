using System.Security.Claims;

namespace MyFinances.Infrastructure.Context;

internal static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out Guid parsedUserId)
            ? parsedUserId
            : throw new ApplicationException("User id is unavailable");
    }

    public static string GetUserEmail(this ClaimsPrincipal? principal)
    {
        string? userEmail = principal?.FindFirstValue(ClaimTypes.Email);

        return userEmail ?? throw new ApplicationException("User email is unavailable");
    }
}