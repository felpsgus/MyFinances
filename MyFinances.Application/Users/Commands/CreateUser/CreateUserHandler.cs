using MediatR;
using MyFinances.Application.Abstractions.Repositories;
using MyFinances.Domain.Entities;

namespace MyFinances.Application.Users.Commands.CreateUser;

public sealed class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(
            request.Name,
            request.Email,
            request.BirthDate,
            request.Password);
        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}