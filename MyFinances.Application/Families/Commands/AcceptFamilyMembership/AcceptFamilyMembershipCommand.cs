using MediatR;

namespace MyFinances.Application.Families.Commands.AcceptFamilyMembership;

public record AcceptFamilyMembershipCommand(Guid FamilyId) : IRequest;