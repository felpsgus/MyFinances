using MyFinances.Domain.Entities;
using MyFinances.Domain.Enum;

namespace MyFinances.Application.Namespaces.Views;

public class NamespaceItemViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public NamespaceType Type { get; set; }

    public Guid? UserId { get; set; }

    public Guid? FamilyId { get; set; }

    public static implicit operator NamespaceItemViewModel(Namespace @namespace)
    {
        return new NamespaceItemViewModel
        {
            Id = @namespace.Id,
            Name = @namespace.Name,
            Type = @namespace.Type,
            UserId = @namespace.UserId,
            FamilyId = @namespace.FamilyId,
        };
    }
}