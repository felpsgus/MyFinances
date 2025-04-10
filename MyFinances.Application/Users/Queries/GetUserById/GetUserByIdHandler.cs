using MediatR;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Abstractions.Repositories;
using MyFinances.Application.Users.Views;
using MyFinances.Domain.Exceptions;

namespace MyFinances.Application.Users.Queries.GetUserById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserViewModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;

    public GetUserByIdHandler(IUserRepository userRepository, IUserContext userContext)
    {
        _userRepository = userRepository;
        _userContext = userContext;
    }

    public async Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found.");

        return UserViewModel.FromEntity(user);
    }
}