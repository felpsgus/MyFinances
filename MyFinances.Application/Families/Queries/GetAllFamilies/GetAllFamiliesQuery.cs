using MediatR;
using MyFinances.Application.Families.Views;

namespace MyFinances.Application.Families.Queries.GetAllFamilies;

public record GetAllFamiliesQuery() : IRequest<List<FamilyViewModel>>;