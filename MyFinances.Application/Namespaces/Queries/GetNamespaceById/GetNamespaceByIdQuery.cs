using MediatR;

namespace MyFinances.Application.Namespaces.Queries.GetNamespaceById;

public record GetNamespaceByIdQuery(Guid NamespaceId) : IRequest<GetNamespaceByIdResponse?>;