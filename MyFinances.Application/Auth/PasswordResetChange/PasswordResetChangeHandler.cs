using MediatR;
using Microsoft.Extensions.Caching.Memory;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;

namespace MyFinances.Application.Auth.PasswordResetChange;

public class PasswordResetChangeHandler : IRequestHandler<PasswordResetChangeCommand>
{
    private readonly IMemoryCache _cache;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PasswordResetChangeHandler(IMemoryCache cache, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _cache = cache;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PasswordResetChangeCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"PasswordResetToken_{request.Email}";
        if (!_cache.TryGetValue(cacheKey, out string token) || token != request.Token)
            throw new InvalidOperationException("Invalid or expired password reset token.");

        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
            throw new NotFoundException(typeof(User));

        user.UpdatePassword(request.Password);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _cache.Remove(cacheKey);
    }
}