using MyFinances.Domain.Entities;
using MyFinances.Domain.Enum;

namespace MyFinances.Application.Namespaces.Queries.GetNamespaceById;

public class GetNamespaceByIdResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public NamespaceType Type { get; set; }

    public Guid? UserId { get; set; }

    public Guid? FamilyId { get; set; }

    public static implicit operator GetNamespaceByIdResponse(Namespace @namespace)
    {
        return new GetNamespaceByIdResponse
        {
            Id = @namespace.Id,
            Name = @namespace.Name,
            Type = @namespace.Type,
            UserId = @namespace.UserId,
            FamilyId = @namespace.FamilyId,
        };
    }
}