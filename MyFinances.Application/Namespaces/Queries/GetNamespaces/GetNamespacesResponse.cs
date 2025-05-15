using MyFinances.Domain.Entities;

namespace MyFinances.Application.Namespaces.Queries.GetNamespaces;

public class GetNamespacesResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public static implicit operator GetNamespacesResponse(Namespace @namespace)
    {
        return new GetNamespacesResponse
        {
            Id = @namespace.Id,
            Name = @namespace.Name,
        };
    }
}