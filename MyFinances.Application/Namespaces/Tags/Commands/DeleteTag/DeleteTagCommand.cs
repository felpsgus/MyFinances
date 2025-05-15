using System.Text.Json.Serialization;
using MyFinances.Application.Abstractions.Interfaces;

namespace MyFinances.Application.Namespaces.Tags.Commands.DeleteTag;

public record DeleteTagCommand() : ICommand
{
    public DeleteTagCommand(Guid namespaceId, Guid tagId) : this()
    {
        NamespaceId = namespaceId;
        TagId = tagId;
    }

    [JsonIgnore]
    public Guid NamespaceId { get; init; }

    public Guid TagId { get; init; }
}