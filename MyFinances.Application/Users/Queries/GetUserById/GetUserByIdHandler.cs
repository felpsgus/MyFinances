using MediatR;
using MyFinances.Application.Abstractions.Repositories;
using MyFinances.Application.Users.Views;
using MyFinances.Domain.Exceptions;

namespace MyFinances.Application.Users.Queries.GetUserById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserViewModel>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
            throw new NotFoundException($"User with ID {request.Id} not found.");

        return UserViewModel.FromEntity(user);
    }
}