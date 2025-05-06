using MediatR;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Enum;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;

namespace MyFinances.Application.Namespaces.Commands.CreateNamespace;

public class CreateNamespaceHandler : IRequestHandler<CreateNamespaceCommand, Guid>
{
    private readonly INamespaceRepository _namespaceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFamilyRepository _familyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateNamespaceHandler(
        INamespaceRepository namespaceRepository,
        IUserRepository userRepository,
        IFamilyRepository familyRepository,
        IUnitOfWork unitOfWork)
    {
        _namespaceRepository = namespaceRepository;
        _userRepository = userRepository;
        _familyRepository = familyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateNamespaceCommand request, CancellationToken cancellationToken)
    {
        if (request.Type == NamespaceType.Personal &&
            !await _userRepository.ExistsAsync((Guid)request.UserId, cancellationToken))
            throw new NotFoundException(nameof(User), request.UserId.ToString());

        if (request.Type == NamespaceType.Family &&
            !await _familyRepository.ExistsAsync((Guid)request.FamilyId, cancellationToken))
            throw new NotFoundException(nameof(Family), request.FamilyId.ToString());

        var namespaceInstance = Namespace.Create(
            request.Name,
            request.Type,
            request.UserId,
            request.FamilyId
        );

        await _namespaceRepository.AddAsync(namespaceInstance, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return namespaceInstance.Id;
    }
}