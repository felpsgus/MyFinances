using MediatR;
using MyFinances.Application.Abstractions.Repositories;
using MyFinances.Application.Families.Views;
using MyFinances.Domain.Exceptions;

namespace MyFinances.Application.Families.Queries.GetById;

public class GetByIdHandler : IRequestHandler<GetByIdQuery, FamilyViewModel>
{
    private readonly IFamilyRepository _familyRepository;

    public GetByIdHandler(IFamilyRepository familyRepository)
    {
        _familyRepository = familyRepository;
    }

    public async Task<FamilyViewModel> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var family = await _familyRepository.GetByIdAsync(request.FamilyId, cancellationToken);
        if (family is null)
            throw new NotFoundException($"Family with ID {request.FamilyId} not found.");

        return family;
    }
}