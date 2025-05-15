using MediatR;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;

namespace MyFinances.Application.Namespaces.Tags.Commands.CreateTag;

public class CreateTagHandler : IRequestHandler<CreateTagCommand>
{
    private readonly INamespaceRepository _namespaceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTagHandler(INamespaceRepository namespaceRepository, IUnitOfWork unitOfWork)
    {
        _namespaceRepository = namespaceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var @namespace = await _namespaceRepository.GetByIdAsync(request.NamespaceId, cancellationToken);
        if (@namespace == null)
            throw new NotFoundException(typeof(Namespace), request.NamespaceId);

        @namespace.AddTag(request.Name);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}