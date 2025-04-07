using MyFinances.Application.Abstractions.Interfaces;

namespace MyFinances.Application.Families.Commands;

public record CreateFamilyCommand(string FamilyName) : ICommand<Guid>;