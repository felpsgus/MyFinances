using MediatR;
using MyFinances.Application.Abstractions;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Abstractions.Repositories;
using MyFinances.Domain.Entities;

namespace MyFinances.Application.Users.Commands.InsertUser;

public sealed class InsertUserHandler : IRequestHandler<InsertUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthenticationService _authService;

    public InsertUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IAuthenticationService authService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
    }

    public async Task<Guid> Handle(InsertUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User(
            request.Name,
            request.Email,
            request.BirthDate,
            _authService.ComputeHash(request.Password));
        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}