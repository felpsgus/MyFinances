using MediatR;
using MyFinances.Application.Families.Views;

namespace MyFinances.Application.Families.Queries.GetFamilyById;

public record GetFamilyByIdQuery(Guid FamilyId) : IRequest<FamilyViewModel>;