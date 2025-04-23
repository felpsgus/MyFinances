using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Domain.Enum;

namespace MyFinances.Infrastructure.UserContext;

internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string ExceptionMessage = "User context is unavailable";

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private Guid? _userId;

    public Guid UserId
    {
        get
        {
            _userId ??= _httpContextAccessor.HttpContext?.User.GetUserId() ??
                        throw new ApplicationException(ExceptionMessage);
            return _userId.Value;
        }
    }

    private string? _email;

    public string Email
    {
        get
        {
            _email ??= _httpContextAccessor.HttpContext?.User.GetUserEmail() ??
                       throw new ApplicationException(ExceptionMessage);
            return _email;
        }
    }

    private bool? _isAdmin;

    public bool IsAdmin
    {
        get
        {
            _isAdmin ??= _httpContextAccessor.HttpContext?.User.IsInRole(Domain.Enum.Role.Admin.ToString()) ??
                         throw new ApplicationException(ExceptionMessage);
            return _isAdmin.Value;
        }
    }

    private Role? _role;

    public Role Role
    {
        get
        {
            _role ??= Enum.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value,
                out Role role)
                ? role
                : throw new ApplicationException(ExceptionMessage);
            return _role.Value;
        }
    }
}