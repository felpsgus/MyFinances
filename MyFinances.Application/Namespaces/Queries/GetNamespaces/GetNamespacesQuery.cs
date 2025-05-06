using MediatR;
using MyFinances.Application.Namespaces.Views;

namespace MyFinances.Application.Namespaces.Queries.GetNamespaces;

public record GetNamespacesQuery : IRequest<List<NamespaceItemViewModel>>;