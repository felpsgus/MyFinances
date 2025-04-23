using MediatR;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Families.Views;
using MyFinances.Domain.Repositories;

namespace MyFinances.Application.Families.Queries.GetAllFamilies;

public class GetAllFamiliesHandler : IRequestHandler<GetAllFamiliesQuery, List<FamilyViewModel>>
{
    private readonly IFamilyRepository _familyRepository;
    private readonly IUserContext _userContext;

    public GetAllFamiliesHandler(IFamilyRepository familyRepository, IUserContext userContext)
    {
        _familyRepository = familyRepository;
        _userContext = userContext;
    }

    public async Task<List<FamilyViewModel>> Handle(GetAllFamiliesQuery request, CancellationToken cancellationToken)
    {
        var families = await _familyRepository.GetAllFamiliesAsync(cancellationToken);
        var familyViews = families.Select(f => FamilyViewModel.FromEntity(f, _userContext)).ToList();
        return familyViews;
    }
}