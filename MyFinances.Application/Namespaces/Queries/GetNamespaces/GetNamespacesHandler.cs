using MediatR;
using MyFinances.Domain.Repositories;

namespace MyFinances.Application.Namespaces.Queries.GetNamespaces;

public class GetNamespacesHandler : IRequestHandler<GetNamespacesQuery, List<GetNamespacesResponse>>
{
    private readonly INamespaceRepository _namespaceRepository;

    public GetNamespacesHandler(INamespaceRepository namespaceRepository)
    {
        _namespaceRepository = namespaceRepository;
    }

    public async Task<List<GetNamespacesResponse>> Handle(GetNamespacesQuery request,
        CancellationToken cancellationToken)
    {
        var namespaces = await _namespaceRepository.GetAllAsync(cancellationToken);
        return namespaces.Select(n => (GetNamespacesResponse)n).ToList();
    }
}