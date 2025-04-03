using MediatR;
using Microsoft.Extensions.Caching.Memory;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Abstractions.Repositories;

namespace MyFinances.Application.Auth.PasswordResetRequest;

public class PasswordResetRequestHandler : IRequestHandler<PasswordResetRequestCommand>
{
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;
    private readonly IMemoryCache _cache;

    public PasswordResetRequestHandler(IEmailService emailService, IUserRepository userRepository, IMemoryCache cache)
    {
        _emailService = emailService;
        _userRepository = userRepository;
        _cache = cache;
    }

    public async Task Handle(PasswordResetRequestCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null) return;

        var token = Guid.NewGuid().ToString();
        var cacheKey = $"PasswordResetToken_{user.Email}";
        _cache.Set(cacheKey, token, TimeSpan.FromHours(1));

        await _emailService.SendEmailAsync(user.Email, "Password Reset Request",
            $"Please use the following token to reset your password: {token}");
    }
}