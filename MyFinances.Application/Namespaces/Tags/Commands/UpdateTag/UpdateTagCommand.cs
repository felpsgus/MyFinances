using System.Text.Json.Serialization;
using MyFinances.Application.Abstractions.Interfaces;

namespace MyFinances.Application.Namespaces.Tags.Commands.UpdateTag;

public record UpdateTagCommand : ICommand
{
    [JsonIgnore]
    public Guid TagId { get; init; }

    public string Name { get; init; } = string.Empty;

    [JsonIgnore]
    public Guid NamespaceId { get; init; }
}