using MediatR;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;

namespace MyFinances.Application.Namespaces.Tags.Commands.DeleteTag;

public class DeleteTagHandler : IRequestHandler<DeleteTagCommand>
{
    private readonly INamespaceRepository _namespaceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTagHandler(INamespaceRepository namespaceRepository, IUnitOfWork unitOfWork)
    {
        _namespaceRepository = namespaceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var @namespace = await _namespaceRepository.GetByIdAsync(request.NamespaceId, cancellationToken);
        if (@namespace is null)
            throw new NotFoundException(typeof(Namespace), request.NamespaceId);

        @namespace.RemoveTag(request.TagId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}