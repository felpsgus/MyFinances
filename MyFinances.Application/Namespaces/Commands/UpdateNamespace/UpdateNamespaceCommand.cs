using System.Text.Json.Serialization;
using MyFinances.Application.Abstractions.Interfaces;

namespace MyFinances.Application.Namespaces.Commands.UpdateNamespace;

public record UpdateNamespaceCommand : ICommand
{
    [JsonIgnore] public Guid NamespaceId { get; init; }
    public string Name { get; init; }
};