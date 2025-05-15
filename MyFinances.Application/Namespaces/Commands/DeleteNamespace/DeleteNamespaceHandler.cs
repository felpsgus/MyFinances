using MediatR;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;

namespace MyFinances.Application.Namespaces.Commands.DeleteNamespace;

public class DeleteNamespaceHandler : IRequestHandler<DeleteNamespaceCommand>
{
    private readonly INamespaceRepository _namespaceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteNamespaceHandler(INamespaceRepository namespaceRepository, IUnitOfWork unitOfWork)
    {
        _namespaceRepository = namespaceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteNamespaceCommand request, CancellationToken cancellationToken)
    {
        var @namespace = await _namespaceRepository.GetByIdAsync(request.NamespaceId, cancellationToken);
        if (@namespace == null)
            throw new NotFoundException(typeof(Namespace), request.NamespaceId);

        _namespaceRepository.Delete(@namespace);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}