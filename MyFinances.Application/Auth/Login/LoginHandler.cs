using FluentValidation;
using MediatR;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Abstractions.Repositories;
using MyFinances.Application.Auth.Views;

namespace MyFinances.Application.Auth.Login;

public sealed class LoginHandler : IRequestHandler<LoginCommand, LoginViewModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public LoginHandler(IUserRepository userRepository, IAuthenticationService authService, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginViewModel> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user == null || !user.ValidatePassword(request.Password))
            throw new ValidationException("Invalid credentials");

        var (token, expiration) = _authService.GenerateJwtToken(user);
        var (refreshToken, refreshTokenExpiration) = _authService.GenerateRefreshToken();

        user.UpdateToken(refreshToken, refreshTokenExpiration);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginViewModel(token, refreshToken, expiration);
    }
}