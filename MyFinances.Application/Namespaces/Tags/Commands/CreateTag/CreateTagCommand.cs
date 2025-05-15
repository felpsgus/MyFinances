using System.Text.Json.Serialization;
using MyFinances.Application.Abstractions.Interfaces;

namespace MyFinances.Application.Namespaces.Tags.Commands.CreateTag;

public record CreateTagCommand : ICommand
{
    public string Name { get; init; } = string.Empty;

    [JsonIgnore]
    public Guid NamespaceId { get; init; }
}