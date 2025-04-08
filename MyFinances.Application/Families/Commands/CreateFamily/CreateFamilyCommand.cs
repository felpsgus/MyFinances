using MyFinances.Application.Abstractions.Interfaces;

namespace MyFinances.Application.Families.Commands.CreateFamily;

public record CreateFamilyCommand(string FamilyName) : ICommand<Guid>;