using MyFinances.Application.Abstractions.Interfaces;

namespace MyFinances.Application.Namespaces.Commands.DeleteNamespace;

public record DeleteNamespaceCommand(Guid NamespaceId) : ICommand;