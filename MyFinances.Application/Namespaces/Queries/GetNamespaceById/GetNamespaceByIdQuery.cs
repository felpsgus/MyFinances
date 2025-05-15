using MediatR;
using MyFinances.Application.Namespaces.Views;

namespace MyFinances.Application.Namespaces.Queries.GetNamespaceById;

public record GetNamespaceByIdQuery(Guid NamespaceId) : IRequest<GetNamespaceByIdResponse?>;