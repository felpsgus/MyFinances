using MediatR;
using MyFinances.Application.Users.Views;

namespace MyFinances.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery() : IRequest<UserViewModel>;