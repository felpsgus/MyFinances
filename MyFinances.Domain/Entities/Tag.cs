using MyFinances.Domain.Extensions;
using MyFinances.Domain.Primitives;

namespace MyFinances.Domain.Entities;

public class Tag : AuditEntity
{
    private Tag()
    {
    }

    public string Name { get; private set; }

    public Guid NamespaceId { get; private set; }

    internal static Tag Create(string name, Guid namespaceId)
    {
        name.ThrowIfNullOrEmpty(nameof(name));

        return new Tag
        {
            Name = name,
            NamespaceId = namespaceId
        };
    }
}