using MediatR;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Users.Views;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;

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
            throw new NotFoundException(typeof(User), userId);

        return UserViewModel.FromEntity(user);
    }
}