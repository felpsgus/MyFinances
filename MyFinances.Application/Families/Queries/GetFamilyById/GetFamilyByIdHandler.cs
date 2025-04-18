using MediatR;
using MyFinances.Application.Abstractions.Repositories;
using MyFinances.Application.Families.Views;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;

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
            throw new NotFoundException(nameof(Family), request.FamilyId.ToString());

        return family;
    }
}