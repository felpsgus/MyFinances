using MediatR;
using MyFinances.Application.Families.Views;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;

namespace MyFinances.Application.Families.Queries.GetFamilyById;

public class GetFamilyByIdHandler : IRequestHandler<GetFamilyByIdQuery, FamilyViewModel>
{
    private readonly IFamilyRepository _familyRepository;

    public GetFamilyByIdHandler(IFamilyRepository familyRepository)
    {
        _familyRepository = familyRepository;
    }

    public async Task<FamilyViewModel> Handle(GetFamilyByIdQuery request, CancellationToken cancellationToken)
    {
        var family = await _familyRepository.GetByIdAsync(request.FamilyId, cancellationToken);
        if (family is null)
            throw new NotFoundException(typeof(Family), request.FamilyId);

        return family;
    }
}