using MediatR;
using MyFinances.Application.Namespaces.Views;
using MyFinances.Domain.Repositories;

namespace MyFinances.Application.Namespaces.Queries.GetNamespaceById;

public class GetNamespaceByIdHandler : IRequestHandler<GetNamespaceByIdQuery, NamespaceItemViewModel?>
{
    private readonly INamespaceRepository _namespaceRepository;

    public GetNamespaceByIdHandler(INamespaceRepository namespaceRepository)
    {
        _namespaceRepository = namespaceRepository;
    }

    public async Task<NamespaceItemViewModel?> Handle(GetNamespaceByIdQuery request, CancellationToken cancellationToken)
    {
        var namespaceItem = await _namespaceRepository.GetByIdAsync(request.NamespaceId, cancellationToken);
        return namespaceItem;
    }
}