using MediatR;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Abstractions.Repositories;
using MyFinances.Application.Users.Views;
using MyFinances.Domain.Exceptions;

namespace MyFinances.Application.Users.Queries.GetById;

public class GetByIdHandler : IRequestHandler<GetByIdQuery, UserViewModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;

    public GetByIdHandler(IUserRepository userRepository, IUserContext userContext)
    {
        _userRepository = userRepository;
        _userContext = userContext;
    }

    public async Task<UserViewModel> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);
        if (user == null)
            throw new NotFoundException($"User with ID {_userContext.UserId} not found.");

        return UserViewModel.FromEntity(user);
    }
}