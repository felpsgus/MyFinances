using MediatR;
using MyFinances.Application.Namespaces.Views;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;

namespace MyFinances.Application.Namespaces.Queries.GetNamespaceById;

public class GetNamespaceByIdHandler : IRequestHandler<GetNamespaceByIdQuery, GetNamespaceByIdResponse?>
{
    private readonly INamespaceRepository _namespaceRepository;

    public GetNamespaceByIdHandler(INamespaceRepository namespaceRepository)
    {
        _namespaceRepository = namespaceRepository;
    }

    public async Task<GetNamespaceByIdResponse?> Handle(GetNamespaceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var namespaceItem = await _namespaceRepository.GetByIdAsync(request.NamespaceId, cancellationToken);
        if (namespaceItem == null)
            throw new NotFoundException(typeof(Namespace), request.NamespaceId);

        return namespaceItem;
    }
}