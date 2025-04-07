using MediatR;
using MyFinances.Application.Families.Views;

namespace MyFinances.Application.Families.Queries.GetById;

public record GetByIdQuery(Guid FamilyId) : IRequest<FamilyViewModel>;