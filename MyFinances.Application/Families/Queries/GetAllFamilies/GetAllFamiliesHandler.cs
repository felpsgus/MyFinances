using MediatR;
using MyFinances.Application.Abstractions.Repositories;
using MyFinances.Application.Families.Views;

namespace MyFinances.Application.Families.Queries.GetAllFamilies;

public class GetAllFamiliesHandler : IRequestHandler<GetAllFamiliesQuery, List<FamilyViewModel>>
{
    private readonly IFamilyRepository _familyRepository;

    public GetAllFamiliesHandler(IFamilyRepository familyRepository)
    {
        _familyRepository = familyRepository;
    }

    public Task<List<FamilyViewModel>> Handle(GetAllFamiliesQuery request, CancellationToken cancellationToken)
    {
        return _familyRepository.GetAllFamiliesAsync(cancellationToken);
    }
}