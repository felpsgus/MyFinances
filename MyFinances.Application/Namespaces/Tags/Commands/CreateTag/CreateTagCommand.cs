using System.Text.Json.Serialization;
using MyFinances.Application.Abstractions.Interfaces;

namespace MyFinances.Application.Namespaces.Tags.Commands.CreateTag;

public record CreateTagCommand : ICommand
{
    public CreateTagCommand(Guid namespaceId, string name)
    {
        NamespaceId = namespaceId;
        Name = name;
    }

    public string Name { get; init; }

    [JsonIgnore]
    public Guid NamespaceId { get; init; }
}