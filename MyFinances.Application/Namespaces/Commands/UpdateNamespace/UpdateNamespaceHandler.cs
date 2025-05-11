using MediatR;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;

namespace MyFinances.Application.Namespaces.Commands.UpdateNamespace;

public class UpdateNamespaceHandler : IRequestHandler<UpdateNamespaceCommand>
{
    private readonly INamespaceRepository _namespaceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateNamespaceHandler(INamespaceRepository namespaceRepository, IUnitOfWork unitOfWork)
    {
        _namespaceRepository = namespaceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateNamespaceCommand request, CancellationToken cancellationToken)
    {
        var @namespace = await _namespaceRepository.GetByIdAsync(request.NamespaceId, cancellationToken);
        if (@namespace is null)
            throw new NotFoundException(typeof(Namespace), request.NamespaceId);

        @namespace.Update(request.Name);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}