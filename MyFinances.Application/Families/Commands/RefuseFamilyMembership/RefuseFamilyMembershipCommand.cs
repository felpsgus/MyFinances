using MediatR;

namespace MyFinances.Application.Families.Commands.RefuseFamilyMembership;

public record RefuseFamilyMembershipCommand(Guid FamilyId) : IRequest;