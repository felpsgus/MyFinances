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
        await _namespaceRepository.Delete(request.NamespaceId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}