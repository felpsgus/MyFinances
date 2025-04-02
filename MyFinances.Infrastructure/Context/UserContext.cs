using Microsoft.AspNetCore.Http;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Domain.Enum;

namespace MyFinances.Infrastructure.Context;

internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string ExceptionMessage = "User context is unavailable";

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new ApplicationException(ExceptionMessage);

    public string Email =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUserEmail() ??
        throw new ApplicationException(ExceptionMessage);

    public bool IsAdmin =>
        _httpContextAccessor
            .HttpContext?
            .User
            .IsInRole(RoleEnum.Admin.ToString()) ??
        throw new ApplicationException(ExceptionMessage);
}