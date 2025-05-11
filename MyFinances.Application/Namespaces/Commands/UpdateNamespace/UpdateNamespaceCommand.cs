using System.Text.Json.Serialization;
using MyFinances.Application.Abstractions.Interfaces;

namespace MyFinances.Application.Namespaces.Commands.UpdateNamespace;

public record UpdateNamespaceCommand : ICommand
{
    public UpdateNamespaceCommand(Guid namespaceId, string name)
    {
        NamespaceId = namespaceId;
        Name = name;
    }

    [JsonIgnore] public Guid NamespaceId { get; init; }
    public string Name { get; init; }
};