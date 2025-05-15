using MediatR;

namespace MyFinances.Application.Namespaces.Queries.GetNamespaces;

public record GetNamespacesQuery : IRequest<List<GetNamespacesResponse>>;