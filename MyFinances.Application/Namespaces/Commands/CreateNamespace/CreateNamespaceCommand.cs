using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Domain.Enum;

namespace MyFinances.Application.Namespaces.Commands.CreateNamespace;

public record CreateNamespaceCommand(
    string Name,
    NamespaceType Type,
    Guid? UserId,
    Guid? FamilyId) : ICommand<Guid>;