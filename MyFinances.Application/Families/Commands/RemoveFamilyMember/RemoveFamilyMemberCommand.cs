using MyFinances.Application.Abstractions.Interfaces;

namespace MyFinances.Application.Families.Commands.RemoveFamilyMember;

public record RemoveFamilyMemberCommand(Guid FamilyId, Guid MemberId) : ICommand;