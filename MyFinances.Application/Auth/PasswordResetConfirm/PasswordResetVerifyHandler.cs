using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace MyFinances.Application.Auth.PasswordResetConfirm;

public class PasswordResetVerifyHandler : IRequestHandler<PasswordResetVerifyCommand>
{
    private readonly IMemoryCache _cache;

    public PasswordResetVerifyHandler(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task Handle(PasswordResetVerifyCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"PasswordResetToken_{request.Email}";

        if (!_cache.TryGetValue(cacheKey, out string token) || token != request.Token)
            throw new InvalidOperationException("Invalid or expired password reset token.");

        return Task.CompletedTask;
    }
}