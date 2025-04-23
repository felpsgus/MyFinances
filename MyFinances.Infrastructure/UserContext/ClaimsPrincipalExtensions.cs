using System.Security.Claims;

namespace MyFinances.Infrastructure.UserContext;

internal static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        var userId = principal?.FindFirstValue(ClaimTypes.Sid);

        return Guid.TryParse(userId, out var parsedUserId)
            ? parsedUserId
            : throw new ApplicationException("User id is unavailable");
    }

    public static string GetUserEmail(this ClaimsPrincipal? principal)
    {
        var userEmail = principal?.FindFirstValue(ClaimTypes.Email);

        return userEmail ?? throw new ApplicationException("User email is unavailable");
    }
}