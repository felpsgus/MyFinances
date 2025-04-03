using MediatR;
using MyFinances.Application.Abstractions.Repositories;
using MyFinances.Domain.Entities;

namespace MyFinances.Application.Users.Commands.InsertUser;

public sealed class InsertUserHandler : IRequestHandler<InsertUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public InsertUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(InsertUserCommand request, CancellationToken cancellationToken)
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