using MediatR;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Abstractions.Repositories;
using MyFinances.Domain.Entities;

namespace MyFinances.Application.Families.Commands;

public class CreateFamilyHandler : IRequestHandler<CreateFamilyCommand, Guid>
{
    private readonly IFamilyRepository _familyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public CreateFamilyHandler(IFamilyRepository familyRepository, IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _familyRepository = familyRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Guid> Handle(CreateFamilyCommand request, CancellationToken cancellationToken)
    {
        var family = new Family(request.FamilyName);
        family.AddFamilyMember(_userContext.UserId);

        await _familyRepository.AddAsync(family, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return family.Id;
    }
}