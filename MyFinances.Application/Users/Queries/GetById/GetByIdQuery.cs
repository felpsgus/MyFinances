using MediatR;
using MyFinances.Application.Users.Views;

namespace MyFinances.Application.Users.Queries.GetById;

public record GetByIdQuery() : IRequest<UserViewModel>;