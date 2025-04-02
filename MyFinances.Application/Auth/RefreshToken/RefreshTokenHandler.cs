using FluentValidation;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Abstractions.Repositories;
using MyFinances.Application.Auth.Views;

namespace MyFinances.Application.Auth.RefreshToken;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, LoginViewModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenHandler(IUserRepository userRepository, IAuthenticationService authService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginViewModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetUserIdFromExpiredToken(request.Token);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
            throw new SecurityTokenException("Invalid token");

        if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiration < DateTime.UtcNow)
            throw new ValidationException("Invalid refresh token");

        var (token, expiration) = _authService.GenerateJwtToken(user);
        var (refreshToken, refreshTokenExpiration) = _authService.GenerateRefreshToken();

        user.UpdateToken(refreshToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginViewModel(token, refreshToken, expiration);
    }
}