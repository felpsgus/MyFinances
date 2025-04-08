using MyFinances.Application.Abstractions.Interfaces;

namespace MyFinances.Application.Families.Commands.AddFamilyMember;

public record AddFamilyMemberCommand(Guid FamilyId, string UserEmail) : ICommand;