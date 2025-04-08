using MediatR;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Abstractions.Repositories;
using MyFinances.Domain.Exceptions;

namespace MyFinances.Application.Families.Commands.RefuseFamilyMembership;

public class RefuseFamilyMembershipHandler : IRequestHandler<RefuseFamilyMembershipCommand>
{
    private readonly IFamilyRepository _familyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public RefuseFamilyMembershipHandler(
        IFamilyRepository familyRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _familyRepository = familyRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task Handle(RefuseFamilyMembershipCommand request, CancellationToken cancellationToken)
    {
        var family = await _familyRepository.GetByIdAsync(request.FamilyId, cancellationToken);
        if (family == null)
            throw new NotFoundException($"Family with ID {request.FamilyId} not found.");

        var membership = family.FamilyMembers.FirstOrDefault(fm => fm.UserId == _userContext.UserId);
        membership.Reject();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}