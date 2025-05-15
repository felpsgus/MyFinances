using MediatR;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;

namespace MyFinances.Application.Namespaces.Tags.Commands.UpdateTag;

public class UpdateTagHandler : IRequestHandler<UpdateTagCommand>
{
    private readonly INamespaceRepository _namespaceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTagHandler(INamespaceRepository namespaceRepository, IUnitOfWork unitOfWork)
    {
        _namespaceRepository = namespaceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var @namespace = await _namespaceRepository.GetByIdAsync(request.NamespaceId, cancellationToken);
        if (@namespace is null)
            throw new NotFoundException(typeof(Namespace), request.NamespaceId);

        var tag = @namespace.Tags.FirstOrDefault(t => t.Id == request.TagId);
        if (tag is null)
            throw new NotFoundException(typeof(Tag), request.TagId);

        tag.Update(request.Name);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}